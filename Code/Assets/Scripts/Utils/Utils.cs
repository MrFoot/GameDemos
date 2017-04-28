using UnityEngine;
using System;
using System.IO;
using System.Collections;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

public class Utils
{
    public static float GetAnimatorTime(GameObject obj)
    {
        Animator animator = obj.GetComponentInChildren<Animator>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        return clips[0].length;
    }

    public static float GetAnimatorTime(Animator ani)
    {
        if (ani == null) return 0;

        AnimationClip[] clips = ani.runtimeAnimatorController.animationClips;

        return clips[0].length;
    }

    public static long GetDecompressedFileSize(string zipPath, string directoryPath)
    {
        long num = 0L;
        if (!string.IsNullOrEmpty(zipPath))
        {
            ZipEntry entry;
            ZipInputStream stream = new ZipInputStream(File.OpenRead(zipPath));
            if (stream == null)
            {
                return 0L;
            }
            while ((entry = stream.GetNextEntry()) != null)
            {
                if (!string.IsNullOrEmpty(Path.GetFileName(entry.Name)))
                {
                    num += entry.Size;
                }
            }
            stream.Close();
        }
        return num;
    }

    public static float GetParticleSystemDuration(GameObject obj)
    {
        ParticleSystem[] particleSystems = obj.GetComponentsInChildren<ParticleSystem>();
        float maxDuration = 0;
        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps.emission.enabled)
            {
                //if (ps.loop)
                //{
                //    return -1f;
                //}
                float dunration = 0f;
                //if (ps.emissionRate <= 0)
                //{
                //    dunration = ps.startDelay + ps.startLifetime;
                //}
                //else
                {
                    dunration = ps.startDelay + Mathf.Max(ps.duration, ps.startLifetime);
                }
                if (dunration > maxDuration)
                {
                    maxDuration = dunration;
                }
            }
        }
        return maxDuration;
    }

    public static void StopParticleSystem(GameObject obj)
    {
        ParticleSystem[] particle = obj.GetComponentsInChildren<ParticleSystem>();
        if (particle != null)
        {
            for (int i = 0; i < particle.Length; ++i)
            {
                particle[i].Stop();
            }
        }
        obj.SetActive(false);
    }

    public static void PlayParticleSystem(GameObject obj)
    {
        obj.SetActive(true);
        ParticleSystem[] particle = obj.GetComponentsInChildren<ParticleSystem>();
        if (particle != null)
        {
            for (int i = 0; i < particle.Length; ++i)
            {
                particle[i].Play();
            }
        }
    }

    public static void PauseParticleSystem(GameObject obj)
    {
        obj.SetActive(true);
        ParticleSystem[] particle = obj.GetComponentsInChildren<ParticleSystem>();
        if (particle != null)
        {
            for (int i = 0; i < particle.Length; ++i)
            {
                particle[i].Pause();
            }
        }
    }

    public static DateTime Unix2DateTime(string unixTimeStamp) {
        
        long t;

        if (long.TryParse(unixTimeStamp, out t))
        {
            return Unix2DateTime(t);
        }
        else
        {
            return DateTime.MinValue;
        }
    }

    public static DateTime Unix2DateTime(long unixTimeStamp) {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        DateTime dt = startTime.AddSeconds(unixTimeStamp);

        return dt;
    }

    public static long DateTime2Unix(DateTime dt) {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        long timeStamp = (long)(dt - startTime).TotalSeconds; // 相差秒数

        return timeStamp;
    }
}
