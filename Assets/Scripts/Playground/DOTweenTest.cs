using UnityEngine;
using DG.Tweening;

public class DOTweenTest : MonoBehaviour {

    private Sequence s;
    private Sequence s_1;

	void Start () {
        s = DOTween.Sequence();

        transform.position = new Vector3(-3.0f, 0.0f);

        s.Append(transform.DOMoveX(3.0f, 3.0f));
        s.Append(GetComponent<Renderer>().material.DOColor(Color.clear, 2.0f));
        s.OnComplete(() => { s_1.Play(); Debug.Log("s.complete"); });

        s_1 = DOTween.Sequence();

		s.Append(GetComponent<Renderer>().material.DOColor(Color.white, 2.0f));
        s.Append(transform.DOMoveX(-3.0f, 3.0f));
        s_1.Pause();
        s_1.OnComplete(() => { s.Play(); Debug.Log("s1.complete"); });
	}

    private void Update()
    {
        if (Input.GetButton("Fire1")) {
            Debug.Log("pressed");
            if (s.IsPlaying()) {
                float elapsed = s.ElapsedPercentage();
                s_1.fullPosition = elapsed * s_1.Duration();
                s_1.Play();
            } else {
                float elapsed = s_1.ElapsedPercentage();
                s.fullPosition = elapsed * s.Duration();
                s.Play();
             }
        }
    }
}
