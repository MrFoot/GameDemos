using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FootStudio.Framework
{
    public class SoundManager : MonoBehaviour
    {

        private const int POOL_SIZE = 25;

        private const float RECYCEL_DT = 2f;

        private const float MUSIC_FADE_DT = 1f;

        private static AudioSource s_BgMusic;

        private static float s_mSoundVolume = 1f;
        private static bool s_mSoundEnabled = true;

        private static float s_mMusicVolume = 1f;
        private static bool s_mMusicEnabled = true;

        public List<AudioClip> PreLoadMusics;

        private static Dictionary<AudioEnum, AudioClip> s_PreLoadDic;

        private static List<AudioSource> s_availablePool;

        private static List<AudioSource> s_playingPool;

        private static GameObject s_audioPoolContainer;

        //private static TweenVolume tween;

        private static AudioListener listener;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            s_BgMusic = gameObject.GetComponent<AudioSource>();

            if (s_BgMusic == null)
            {
                s_BgMusic = gameObject.AddComponent<AudioSource>();
            }

            MusicVolume = 0.6f;//PlayerPrefs.GetFloat("SoundVolume", 0.35f);
            SoundVolume = 1.0f;//PlayerPrefs.GetFloat("MusicVolume", 1.0f);

            SoundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
            MusicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;

            s_availablePool = new List<AudioSource>();

            s_playingPool = new List<AudioSource>();

            s_PreLoadDic = new Dictionary<AudioEnum, AudioClip>();

            listener = gameObject.GetComponent<AudioListener>();

            //预加载声音预设

            for (int i = 0, max = PreLoadMusics.Count ; i < max ; i++)
            {
                int aid;
                if (PreLoadMusics[i] != null && int.TryParse(PreLoadMusics[i].name, out aid))
                {
                    s_PreLoadDic.Add((AudioEnum)aid, PreLoadMusics[i]);
                }
            }

            InitTween();

            InitAudioPool();
        }

        void InitAudioPool()
        {
            s_audioPoolContainer = new GameObject("AudioPoolContainer");

            s_audioPoolContainer.transform.parent = null;

            DontDestroyOnLoad(s_audioPoolContainer);

            for (int i = 0 ; i < POOL_SIZE ; i++)
            {
                GameObject go = new GameObject("Audio_" + i);

                go.transform.parent = s_audioPoolContainer.transform;

                go.transform.localPosition = Vector3.zero;

                AudioSource source = go.AddComponent<AudioSource>();

                source.playOnAwake = false;

                s_availablePool.Add(source);
            }

        }

        void InitTween()
        {
            //tween = this.GetComponent<TweenVolume>();

            //tween.from = 0.02f;

            //tween.duration = MUSIC_FADE_DT;

            //tween.to = s_mMusicVolume;
        }

        //音效音量
        public static float SoundVolume
        {
            get { return s_mSoundVolume; }

            set
            {
                if (s_mSoundVolume != value)
                {
                    s_mSoundVolume = value;
                    PlayerPrefs.SetFloat("SoundVolume", value);
                    PlayerPrefs.Save();
                }
            }

        }

        static bool isTempSound = true;
        public static void ToggleSound(bool enable)
        {
            isTempSound = enable;
        }

        //音效开关
        public static bool SoundEnabled
        {
            get { return s_mSoundEnabled; }

            set
            {
                if (s_mSoundEnabled != value)
                {
                    s_mSoundEnabled = value;
                    PlayerPrefs.SetInt("SoundEnabled", value ? 1 : 0);
                    PlayerPrefs.Save();
                }
            }
        }

        //音乐音量
        public static float MusicVolume
        {
            get { return s_mMusicVolume; }

            set
            {
                if (s_mMusicVolume != value)
                {
                    s_mMusicVolume = value;
                    PlayerPrefs.SetFloat("MusicVolume", value);
                    PlayerPrefs.Save();
                }
            }

        }

        //音乐开关
        public static bool MusicEnabled
        {
            get { return s_mMusicEnabled; }

            set
            {
                if (s_mMusicEnabled != value)
                {
                    s_mMusicEnabled = value;
                    PlayerPrefs.SetInt("MusicEnabled", value ? 1 : 0);
                    PlayerPrefs.Save();
                    if (s_mMusicEnabled)
                    {
                        s_BgMusic.Play();
                    }
                    else
                    {
                        s_BgMusic.Pause();
                    }
                }
            }
        }


        //临时控制音乐，受MusicEnabled影响
        public static bool MusicEnabledTemp
        {
            get
            {
                if (s_BgMusic == null) return false;
                return s_BgMusic.isPlaying;
            }

            set
            {
                if (MusicEnabled)   //本身打开背景音乐
                {

                    if (s_BgMusic != null)
                    {
                        if (value)
                        {
                            if (!s_BgMusic.isPlaying)
                                s_BgMusic.Play();
                        }
                        else
                        {
                            s_BgMusic.Pause();
                        }
                    }
                }
                else                //本身没打开背景音乐
                {
                    //Do Nothing
                }
            }
        }

        #region 声音淡入淡出
        private const float m_lasttime = 3.0f;
        //淡入时间 背景音乐用
        private static float m_value = 0.1f;
        private static float m_time = 3f;
        //淡出时间 飞机用
        private static bool m_flyingplay = false;
        private static float m_flyingvalue = 0.0f;
        private static AudioSource m_flyingsource = null;
        private const float m_flyinglast = 2.0f;


        private float RecycleTimer = 0;


        void FadeSound()
        {
            if (m_time < m_lasttime)
            {
                m_time += Time.deltaTime;
                m_value = Mathf.Lerp(0.0f, MusicVolume, m_time / m_lasttime);
                s_BgMusic.volume = m_value;
            }

            if (m_flyingplay)
            {
                if (m_flyingvalue < m_flyinglast)
                {
                    m_flyingvalue += Time.deltaTime;
                    m_flyingsource.volume = Mathf.Lerp(SoundVolume, 0.0f, m_flyingvalue / m_flyinglast);
                }
                else
                {
                    m_flyingplay = false;
                    m_flyingsource.Stop();
                }
            }
        }

        public static void SetBackGroundMusic(bool enable)
        {
            if (enable)
            {
                //背景音乐设置打开 且 音乐暂停时
                if (MusicEnabled && !s_BgMusic.isPlaying)
                {
                    s_BgMusic.volume = 0f;
                    m_time = 0f;

                    s_BgMusic.Pause();
                    s_BgMusic.Play();
                }
            }
            else
            {
                s_BgMusic.Pause();
            }
        }

        public void SoundFadeOut(AudioEnum ae)
        {
            for (int i = s_playingPool.Count - 1 ; i >= 0 ; i--)
            {
                AudioSource source = s_playingPool[i];

                if (source.clip.name.Equals(((int)ae).ToString()))
                {
                    m_flyingvalue = 0f;
                    m_flyingsource = source;
                    m_flyingplay = true;
                    break;
                }
            }
        }


        #endregion

        void Update()
        {
            //FadeSound();

            RecycleAudio();
        }

        public void RecycleAudio()
        {

            RecycleTimer += Time.fixedDeltaTime;

            if (RecycleTimer < RECYCEL_DT) return;

            RecycleTimer = 0;

            for (int i = s_playingPool.Count - 1 ; i >= 0 ; i--)
            {
                int s = s_availablePool.Count;

                AudioSource source = s_playingPool[i];

                if (source != null && !source.isPlaying)
                {
                    s_playingPool.Remove(source);

                    source.Stop();

                    source.clip = null;

                    source.transform.parent = s_audioPoolContainer.transform;

                    source.transform.localPosition = Vector3.zero;

                    s_availablePool.Add(source);
                }
            }
        }

        static void s_MusicFadeInAndOut()
        {

            //if(tween != null)
            //{
            //    tween.ResetToBeginning();
            //    tween.PlayForward();
            //}
        }

        //播放背景音乐
        public static AudioSource PlayMusic(AudioEnum ae, bool loop = true)
        {

            if (s_BgMusic.isPlaying)
                if (s_BgMusic.clip != null && s_BgMusic.clip.name.Substring(0, 4) == ((int)ae).ToString())
                    return null;

            AudioSource source = playSound(s_BgMusic, (int)ae, 0, MusicVolume, loop, 1, MusicEnabled);

            s_MusicFadeInAndOut();

            return source;
        }

        //播放音效_source
        public static AudioSource PlaySound(AudioEnum ae, AudioSource source, bool loop = false)
        {
            return playSound(source, (int)ae, 0, SoundVolume, loop, 1, SoundEnabled);
        }

        //播放音效_transform
        public static AudioSource PlaySound(AudioEnum ae, Transform trans = null, bool loop = false, float volume = 1)
        {
            if (!SoundEnabled)
                return null;
            if (!isTempSound)
                return null;


            volume = volume * SoundVolume;

            AudioClip clip = null;
            if (!s_PreLoadDic.TryGetValue(ae, out clip))
            {
                Object asset = Resources.Load("Sound/" + (int)ae);
                if (asset != null)
                {
                    clip = AudioClip.Instantiate(asset) as AudioClip;
                    s_PreLoadDic.Add(ae, clip);
                }
            }

            if (clip != null && volume > 0.01f)
            {
                //Create the source    
                AudioSource source = s_availablePool[s_availablePool.Count - 1];
                s_availablePool.RemoveAt(s_availablePool.Count - 1);
                if (source != null)
                {
                    s_playingPool.Add(source);
                    source.clip = clip;
                    source.volume = volume;
                    source.loop = loop;
                    source.pitch = 1;
                    source.Play();

                    GameObject go = source.gameObject;
                    if (trans != null)
                    {
                        //go.transform.parent = trans;
                        go.transform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        //go.transform.parent = null;
                        go.transform.position = Vector3.zero;
                    }

                    if (!loop)
                    {
                        //Debug.LogError("clip.length = " + clip.length);
                        //TimerManager.StartTimer(testTimerManager, 1.0f);
                    }
                }
                return source;
            }
            return null;
        }

        private static AudioSource playSound(AudioSource source, int resourceID, float delay = 0, float volume = 1, bool loop = false, float pitch = 1, bool isPlay = true)
        {
            AudioClip clip = null;
            if (!s_PreLoadDic.TryGetValue((AudioEnum)resourceID, out clip))   //没有预加载
            {
                Object asset = Resources.Load("Sound/" + resourceID);
                if (asset != null)
                {
                    clip = AudioClip.Instantiate(asset) as AudioClip;
                }
            }

            if (source != null && clip != null && volume > 0.01f)
            {
                source.clip = clip;
                source.loop = loop;
                source.volume = volume;
                source.pitch = pitch;
                source.Stop();
                source.playOnAwake = false;

                if (!isPlay) return source;
                if (!isTempSound) return source;

                if (delay > 0)
                {
                    source.PlayDelayed(delay);
                }
                else
                {
                    if (source.isActiveAndEnabled)
                    {
                        source.Play();
                    }
                }

                return source;
            }
            else
            {
                if (clip == null)
                {
                    Debug.LogError("No Such Clip : resourceID = " + resourceID);
                }
                return null;
            }
        }

        //控制Listener
        public static bool ListenerEnabled
        {
            get
            {
                return listener.enabled;
            }

            set
            {
                listener.enabled = value;
            }
        }

    }
}