using UnityEngine;

public class UnbreakableBlock : BaseBlock {

	public override void BallCollision (GameObject ball) {
		PlayBlockSound (destroySound);
		DisplayUnbreakeable ();
    }

    protected override void ShowBallHitParticles(ContactPoint2D contact)
    {
        GameObject hitParticles = GameObjectPool.Spawn(ballHitParticlePool, transform.position, Quaternion.identity);


        ParticleSystem particleSys = hitParticles.GetComponent<ParticleSystem>();
        particleSys.Play();

        StartCoroutine(PutBackParticles(particleSys));
    }

	private void DisplayUnbreakeable () {
		// ... hacer como que hace fuerza?
	}
}