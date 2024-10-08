using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		float scroll = Input.GetAxis("Mouse ScrollWheel");

			Debug.Log("Mouse ScrollWheel: " + scroll);
			// 处理滚轮滚动事件，例如缩放


	}
}
