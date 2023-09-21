using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    void Start()
    {
        FitCamera();
    }

    /// <summary>
    /// 背景がカメラの正面に来るように設定
    /// </summary>
    void FitCamera()
    {
        var cameraTr = Camera.main?.transform;
        if (cameraTr)
        {
            var cameraPos = cameraTr.position;
            transform.position = new Vector3(cameraPos.x, cameraPos.y, transform.position.z);
        }
    }
}
