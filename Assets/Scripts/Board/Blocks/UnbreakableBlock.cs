using UnityEngine;

public class UnbreakableBlock : BaseBlock {
    private static int AppearHash = Animator.StringToHash("Appear");
    private static int DisappearHash = Animator.StringToHash("Disappear");

	public override void BallCollision (GameObject ball) {
		PlayBlockSound (destroySound);
    }

    protected override void ShowBallHitParticles(ContactPoint2D contact)
    {
        GameObject hitParticles = GameObjectPool.Spawn(ballHitParticlePool, transform.position, Quaternion.identity);


        ParticleSystem particleSys = hitParticles.GetComponent<ParticleSystem>();
        particleSys.Play();

        StartCoroutine(PutBackParticles(particleSys));
    }

    public override void Activate()
    {
        Debug.Log("Activate");
        base.Activate();
        GetComponent<Animator>().SetTrigger(AppearHash);
    }

    public override void Deactivate()
    {
        Debug.Log("Deactivate");
        GetComponent<Animator>().SetTrigger(DisappearHash);
    }
}