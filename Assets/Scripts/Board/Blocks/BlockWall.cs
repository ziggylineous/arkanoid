using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockWall : MonoBehaviour {

    public List<BaseBlock> wallBlocks;
    private LevelConditionMacro openConditions;
    Sequence tween = null;

	void Start () {
        foreach (BaseBlock block in wallBlocks)
            block.Destroyed += b => wallBlocks.Remove(b);

        openConditions = new LevelConditionMacro(gameObject, Open, Close);

	}

    private void Open() {
        wallBlocks.ForEach(block => block.GetComponent<Collider2D>().enabled = false);

        if (tween != null)
            tween.Complete(true);

        tween = OpenTween;
    }
	
    private void Close() {
        wallBlocks.ForEach(block => block.gameObject.SetActive(true));

        if (tween != null)
            tween.Complete(true);

        tween = CloseTween;
    }

    private Sequence OpenTween {
        get {
            Sequence openTween = DOTween.Sequence();

            float elapsed = 0.0f;
            float Delay = 0.1f;
            
            foreach (BaseBlock block in wallBlocks)
            {
                Material blockMaterial = block.GetComponent<Renderer>().material;
                Tweener fadeOut = blockMaterial.DOFade(0.0f, 0.25f);
                openTween.Insert(elapsed, fadeOut);

                elapsed += Delay;
            }

			openTween.OnComplete(
				() => wallBlocks.ForEach(block => block.gameObject.SetActive(false))
			);
			
			return openTween;
        }
    }

    private Sequence CloseTween
    {
        get
        {
            Sequence closeTween = DOTween.Sequence();

            float elapsed = 0.0f;
            float Delay = 0.1f;

            foreach (BaseBlock block in wallBlocks)
            {
                Material blockMaterial = block.GetComponent<Renderer>().material;

                Tweener fadeIn = blockMaterial.DOFade(1.0f, 0.25f);
                closeTween.Insert(elapsed, fadeIn);

                elapsed += Delay;
            }

            closeTween.OnComplete(
                () => wallBlocks.ForEach(block => block.GetComponent<Collider2D>().enabled = true)
            );

            return closeTween;
        }
    }
}
