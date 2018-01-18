using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class BaseBlock : MonoBehaviour {
	public int score = 0;
	public AudioClip destroySound;

	public delegate void Delegate(BaseBlock block);
	public event Delegate Destroyed;

	public abstract void BallCollision (GameObject ball);

    public GameObject ballHitParticlePool;

    private static ContactPoint2D[] ballContacts = new ContactPoint2D[4];

	void OnCollisionEnter2D(Collision2D collision) {
        collision.GetContacts(ballContacts);
        ShowBallHitParticles(ballContacts[0]);
		
		PlayBlockSound (destroySound);

        BallCollision(collision.gameObject);
    }

    public void BlockDestroy() {
		BreakInPieces ();
		Destroy (gameObject);
		Destroyed(this);
	}

	protected void PlayBlockSound(AudioClip sound) {
        GameObject blockManager = GameObject.Find("Level/Blocks"); // TODO: ver q puede hacerse
        blockManager.GetComponent<BlockSoundEffects> ().Play (sound);
	}

    protected virtual void ShowBallHitParticles(ContactPoint2D contact) {
        Vector3 particlesPosition = contact.point;

        GameObject hitParticles = GameObjectPool.Spawn(ballHitParticlePool, particlesPosition, Quaternion.identity);


        ParticleSystem particleSys = hitParticles.GetComponent<ParticleSystem>();
        float angle = Mathf.Atan2(-contact.normal.y, -contact.normal.x) * Mathf.Rad2Deg;
        //Debug.Log("normal angle = "+ angle);
        //Debug.DrawRay(contact.point, -contact.normal, Color.white, 10.0f);
        ParticleSystem.ShapeModule shapeModule = particleSys.shape;
        shapeModule.rotation = new Vector3(0.0f, 0.0f, angle - 45.0f);

        particleSys.Play();

        StartCoroutine(PutBackParticles(particleSys));
    }

    protected IEnumerator PutBackParticles(ParticleSystem particles) {
        yield return new WaitForSeconds(particles.main.duration);

        GameObjectPool.Despawn(particles.gameObject);
    }

	private static GameObject blockPartPrefab = null;

	public void BreakInPieces() {
		if (blockPartPrefab == null) {
			blockPartPrefab = (GameObject) Resources.Load ("Prefabs/Blocks/Part", typeof(GameObject));
		}

		Vector3 centerVert = new Vector3 (
			Random.Range(-0.33f, 0.33f),
			Random.Range(-0.33f, 0.33f),
			0.0f
        );

		Vector3 topVert = new Vector3 (
			Random.Range(-0.4f, 0.4f),
			0.5f,
			0.0f
		);

		Vector3 bottomVert = new Vector3 (
			Random.Range(-0.4f, 0.4f),
			-0.5f,
			0.0f
		);

		Vector3 leftVert = new Vector3 (
			-0.5f,
			Random.Range(-0.4f, 0.4f),
			0.0f
		);

		Vector3 rightVert = new Vector3 (
			0.5f,
			Random.Range(-0.4f, 0.4f),
			0.0f
		);

		GameObject blockPartTopLeft = Instantiate(blockPartPrefab, transform.position, Quaternion.identity);

		MeshFilter meshFilter = blockPartTopLeft.GetComponent<MeshFilter> ();
		Mesh mesh = meshFilter.mesh;

		Vector3[] vertices = mesh.vertices;
		vertices[0] = leftVert;
		vertices [1] = topVert;
		vertices[2] = centerVert;
		vertices[3] = new Vector3(-0.5f, 0.5f, 0.0f);
		mesh.vertices = vertices;



		GameObject blockPartTopRight = Instantiate(blockPartPrefab, transform.position, Quaternion.identity);

		meshFilter = blockPartTopRight.GetComponent<MeshFilter> ();
		mesh = meshFilter.mesh;

		vertices = mesh.vertices;
		vertices[0] = centerVert;
		vertices [1] = new Vector3(0.5f, 0.5f, 0.0f);
		vertices[2] = rightVert;
		vertices[3] = topVert;
		mesh.vertices = vertices;


		GameObject blockPartBottomLeft = Instantiate(blockPartPrefab, transform.position, Quaternion.identity);

		meshFilter = blockPartBottomLeft.GetComponent<MeshFilter> ();
		mesh = meshFilter.mesh;

		vertices = mesh.vertices;
		vertices [0] = new Vector3(-0.5f, -0.5f, 0.0f);
		vertices[1] = centerVert;
		vertices[2] = bottomVert;
		vertices[3] = leftVert;
		mesh.vertices = vertices;



		GameObject blockPartBottomRight = Instantiate(blockPartPrefab, transform.position, Quaternion.identity);

		meshFilter = blockPartBottomRight.GetComponent<MeshFilter> ();
		mesh = meshFilter.mesh;

		vertices = mesh.vertices;
		vertices[0] = bottomVert;
		vertices[1] = rightVert;
		vertices [2] = new Vector3(0.5f, -0.5f, 0.0f);
		vertices[3] = centerVert;
		mesh.vertices = vertices;

		Dictionary<GameObject, Vector3> partsMoveDirs = new Dictionary<GameObject, Vector3> (4);
		partsMoveDirs.Add (blockPartTopLeft, new Vector3 (-1.0f, 1.0f));
		partsMoveDirs.Add (blockPartTopRight, new Vector3 (1.0f, 1.0f));
		partsMoveDirs.Add(blockPartBottomLeft, new Vector3(-1.0f, -1.0f));
		partsMoveDirs.Add (blockPartBottomRight, new Vector3 (1.0f, -1.0f));
		float moveMagnitude = 0.5f;

		foreach (KeyValuePair<GameObject, Vector3> partDir in partsMoveDirs) {
			partDir.Key.transform.DOMove (partDir.Value * moveMagnitude, 1.0f).SetRelative ().SetEase(Ease.OutQuart);
			Material material = partDir.Key.GetComponent<MeshRenderer> ().material;
			material.DOFade (0.0f, 1.0f).SetEase(Ease.OutQuart);
		}
	}
}