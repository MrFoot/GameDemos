using UnityEngine;
using System.Collections;

public class ScreenShoot : MonoBehaviour {

	public Camera ShootCamera;
    //public UILabel Name;
    //public UILabel Score;

    //public UITexture Texture;

	private int height = 1136;
	private int width = 640;

	public void Shoot() {
        //Name.text = CommonSmallGameUIDataManager.Instance.RankNames[(int)CommonSmallGameUIDataManager.Instance.GameType];
        //Score.text = CommonSmallGameUIDataManager.Instance.ScoreNumber.ToString();
        //Texture.mainTexture = ShareManager.encoded;

		Main.Instance.ShareManager.GetEncodeTexture ();
		StartCoroutine (screenShoot());
	}

	IEnumerator screenShoot() {
		yield return new WaitForEndOfFrame ();
		Rect rect = new Rect (0, 0, width-2, height); //防止黑边
		// 创建一个RenderTexture对象
		RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
		// 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机
		ShootCamera.targetTexture = rt;
		ShootCamera.Render();
		//ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。
		//ps: camera2.targetTexture = rt;
		//ps: camera2.Render();
		//ps: -------------------------------------------------------------------
		
		// 激活这个rt, 并从中中读取像素。
		RenderTexture before = RenderTexture.active;
		RenderTexture.active = rt;
		Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
		screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素
		//screenShot.Apply();
		
		// 重置相关参数，以使用camera继续在屏幕上显示
		ShootCamera.targetTexture = null;
		//ps: camera2.targetTexture = null;
		RenderTexture.active = before; // JC: added to avoid errors
		GameObject.Destroy(rt);
		// 最后将这些纹理数据，成一个png图片文件
		byte[] bytes = screenShot.EncodeToPNG();
		string filename = Main.Instance.SDKManager.GetImagePath() + "/Screenshot.png";
		System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("截屏了一张照片: {0}", filename));
		Destroy(this.gameObject);

		Main.Instance.SDKManager.ShareImage (filename);
	}
}
