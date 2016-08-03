using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
using System.Reflection;
using UnityEditor.Animations;

public class CreateAnimator : Editor {

	public class AnimClipInfo
	{
		public int startFrame = 0;
		public int endFrame = 2;
		public int recoverFrame = 1;
		public bool loop = false;
		public string name = "";
	}

	[MenuItem("AssetBundles/拆分模型")]
	static void ClipAnimation() {
		UnityEngine.Object[] selection = Selection.GetFiltered (typeof(UnityEngine.Object), SelectionMode.DeepAssets);
		if (selection == null || selection.Length < 1)
		{
			Debug.LogError("需要选择一个模型");
			return;
		}

		for (int i = 0; i < selection.Length; i++) {
			GameObject o = selection [i] as GameObject;

			if (o == null) {
				continue;
			}

			string assetPath = AssetDatabase.GetAssetPath (o);
			string rootPath = assetPath.Substring (0, assetPath.LastIndexOf ('/') + 1);
			string name = o.name;

			if (name.Contains ("@"))
				continue;
			if (!assetPath.Contains ("/Characters/"))
				continue;
			if (!assetPath.ToLower ().EndsWith (".fbx"))
				continue;

			ModelImporter mi = AssetImporter.GetAtPath(assetPath) as ModelImporter;
			string animInfoPath = rootPath + name + ".xml";
			ModelImporterClipAnimation animOri = null;

			if (mi.clipAnimations != null && mi.defaultClipAnimations.Length > 0)
			{
				animOri = mi.defaultClipAnimations[0];
			}
			else
			{
				Debug.Log("No Original Animation Clips");
			}

			ModelImporterClipAnimation[] clips = SpliteClips(animInfoPath,animOri);
			if (clips != null)
			{
				mi.clipAnimations = clips;
			}
			else
			{
				Debug.Log("Can't find valid clips in config XML!");
			}

			FixMask (mi);

			AssetDatabase.ImportAsset (assetPath);

			CreatePrefab (name, o);

			ReflashAnimatorController (name, assetPath);
		}

		AssetDatabase.Refresh ();

		EditorUtility.DisplayDialog("提示", "动画拆分完成！", "确定");
	}

	public static ModelImporterClipAnimation[] SpliteClips(string path, ModelImporterClipAnimation refClip)
	{
		string takeName = "Take 001";

		if (refClip != null)
		{
			takeName = refClip.takeName;
		}

		List<AnimClipInfo> clipInfo = new List<AnimClipInfo>();
		XmlDocument xml = null;
		if (File.Exists(path))
		{
			xml = new XmlDocument();
			xml.Load(path);
		}

		if (xml == null)
		{
			Debug.LogError ("xml文件不存在");
			return null;
		}

		XmlNodeList nodeList = xml.SelectSingleNode("character").ChildNodes;

		foreach (XmlElement xe in nodeList)
		{
			int enabled = 0;
			int start = -1;
			int end = -1;
			int loop = 0;
			int.TryParse(xe.GetAttribute("enable"), out enabled);
			int.TryParse(xe.GetAttribute("start_frame"), out start);
			int.TryParse(xe.GetAttribute("end_frame"), out end);
			int.TryParse(xe.GetAttribute("loop"), out loop);

			if (enabled != 0 && start >= 0 && end > 0)
			{
				AnimClipInfo clip = new AnimClipInfo();
				clip.name = xe.GetAttribute("name");
				clip.name = clip.name.ToLower();
				clip.name = clip.name.Trim();
				clip.loop = (loop == 1);
				clip.startFrame = start;
				clip.endFrame = end;
				clipInfo.Add(clip);
			}
		}

		ModelImporterClipAnimation[] newClips = new ModelImporterClipAnimation[clipInfo.Count];
		for (int i = 0; i < clipInfo.Count; ++i)
		{
			newClips[i] = new ModelImporterClipAnimation();
			newClips[i].takeName = takeName;
			newClips[i].name = clipInfo[i].name;
			newClips[i].loopTime = clipInfo[i].loop;

			newClips[i].firstFrame = clipInfo[i].startFrame;
			newClips[i].lastFrame = clipInfo[i].endFrame;
		}
		return newClips;
	}

	static void FixMask(ModelImporter mi) {
		Type modelImporterType = typeof(ModelImporter);
		MethodInfo updateTransformMaskMethodInfo = modelImporterType.GetMethod("UpdateTransformMask", BindingFlags.NonPublic | BindingFlags.Static);

		ModelImporterClipAnimation[] clipAnimations = mi.clipAnimations;
		SerializedObject so = new SerializedObject(mi);
		SerializedProperty sclips = so.FindProperty("m_ClipAnimations");

		AvatarMask avatarMask = new AvatarMask();
		avatarMask.transformCount = mi.transformPaths.Length;
		for(int j=0; j<mi.transformPaths.Length; j++ )
		{
			avatarMask.SetTransformPath(j,mi.transformPaths[j]);
			avatarMask.SetTransformActive(j,true);
		}

		for(int j=0; j<clipAnimations.Length; j++ )
		{
			SerializedProperty transformMaskProperty = sclips.GetArrayElementAtIndex(j).FindPropertyRelative("transformMask");
			updateTransformMaskMethodInfo.Invoke(mi, new System.Object[]{avatarMask, transformMaskProperty});
		}
		so.ApplyModifiedProperties();
	}

	static void CreatePrefab(string name, GameObject o) {
		// 创建prefab  
		string prefabPath = "Assets/Resources/Bear/" + name + ".prefab";  
		GameObject go = GameObject.Instantiate(o) as GameObject;
		go.name = name;  
		Animator animator = go.GetComponent<Animator>();  
		if (animator == null) {  
			animator = go.AddComponent<Animator>();  
		}  
		go.AddComponent<BearCharacterAnimationEvents> ();
		go.AddComponent<CharacterData> ();
		go.AddComponent<BearSlotController> ();

		animator.applyRootMotion = false;  //使用脚本控制transform，而不是动画

		string conPath = "Assets/Resources/Bear/" + name + ".controller";

		if (!File.Exists (conPath)) {
			Debug.LogError (name + "没有动画控制文件");
			GameObject.DestroyImmediate(go, true);  
			return;
		}
		AnimatorController animatorController = AssetDatabase.LoadAssetAtPath<AnimatorController> (conPath);
		animator.runtimeAnimatorController = animatorController;  
		PrefabUtility.CreatePrefab(prefabPath, go);  
		GameObject.DestroyImmediate(go, true);  

		AssetDatabase.SaveAssets();  
	}

	static void ReflashAnimatorController(string name, string assetPath) {
		string path = "Assets/Resources/Bear/" + name + ".controller";
		if (!File.Exists (path)) {
			Debug.LogError (name + "没有动画控制文件");
			return;
		}
		AnimatorController animatorController = AssetDatabase.LoadAssetAtPath<AnimatorController> (path);
		AnimatorControllerLayer layer = animatorController.layers[0];
		AnimatorStateMachine machine=layer.stateMachine; 

		List<AnimationClip> clips = new List<AnimationClip>(); 
		UnityEngine.Object[] objects = AssetDatabase.LoadAllAssetsAtPath(assetPath);  
		for(int m=0;m<objects.Length;m++)  
		{  
			if(objects[m].GetType()==typeof(AnimationClip) && !objects[m].name.Contains("Take 001"))  
			{  
				AnimationClip clip=(AnimationClip)objects[m];  
				if (clip != null) {
					clips.Add (clip);
				}
			}  
		} 
		CheckAndRefreshAnimatorController (clips, machine);
	}

	static void CheckAndRefreshAnimatorController(List<AnimationClip> clips, AnimatorStateMachine stateMachine)
	{ 
		for (int i = 0; i < stateMachine.states.Length; i++) {
			ChildAnimatorState childState = stateMachine.states[i];  
			if(childState.state.motion == null)  
			{  
				Debug.LogError("Null motion : " + childState.state.name);  
				continue;  
			}  

			if(childState.state.motion.GetType() == typeof(AnimationClip))  
			{  
				for(int j = 0; j < clips.Count; j++)  
				{  
					if(clips[j].name.CompareTo(childState.state.motion.name) == 0)  
					{  
						childState.state.motion = (Motion)clips[j];  
						break;  
					}  
				}  
			} else if(childState.state.motion.GetType() == typeof(BlendTree)) {  
				//BlendTree这个类有BUG，不能直接修改Motion, 要先记录原本的信息，再全部删除原本的，再修改，再加上去.  
				List<Motion> allMotion = new List<Motion>();  
				List<float> allThreshold = new List<float>();  
				BlendTree tree = (BlendTree)childState.state.motion;  

				for(int k = 0; k < tree.children.Length; k++)  
				{  
					allMotion.Add(tree.children[k].motion);  
					allThreshold.Add(tree.children[k].threshold);  
				}  

				for(int k = 0; k < allMotion.Count; k++)  
				{  
					if(allMotion[k].GetType() == typeof(AnimationClip))  
					{  
						for(int j = 0; j < clips.Count; j++)  
						{  
							if(clips[j].name.CompareTo(allMotion[k].name) == 0)  
							{  
								allMotion[k] = (Motion)clips[j];  
								break;  
							}  
						}  
					} else if(allMotion[k].GetType() == typeof(BlendTree)) {  
						Debug.LogError("不能多层BlendTree!");  
					}  
				}  

				for(int k = tree.children.Length - 1; k >= 0; k--)  
				{  
					tree.RemoveChild(k);  
				}  

				for(int k = 0; k < allMotion.Count; k++)  
				{  
					tree.AddChild(allMotion[k], allThreshold[k]);  
				}  

			}  
		}

		for(int i = 0; i < stateMachine.stateMachines.Length; i++)   
		{  
			CheckAndRefreshAnimatorController(clips, stateMachine.stateMachines[i].stateMachine);  
		}  
	}
}
