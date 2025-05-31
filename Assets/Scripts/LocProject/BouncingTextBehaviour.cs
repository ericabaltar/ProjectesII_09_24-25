using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BouncingTextBehaviour : MonoBehaviour
{
    TextMeshProUGUI textMesh;
    [SerializeField] AnimationCurve movementCurve;
    [SerializeField] float movementScale = 5.0f;
    [SerializeField] float timeScale = 1.0f;
    [SerializeField] float delayBetweenLetters = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.ForceMeshUpdate();
        var mesh = textMesh.mesh;
        var vertices = mesh.vertices;

        for (int i = 0; i < textMesh.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];

            int index = c.vertexIndex;

            float adjustedTime = (Time.time + i * -delayBetweenLetters) * timeScale;

            float yOffset = movementCurve.Evaluate(adjustedTime - Mathf.Floor(adjustedTime));
            Vector3 offset = Vector3.up * yOffset * movementScale;

            for(int j = 0; j < 4; j++)
                vertices[index + j] += offset;
        }

        mesh.vertices = vertices;
        textMesh.canvasRenderer.SetMesh(mesh);
    }
}
