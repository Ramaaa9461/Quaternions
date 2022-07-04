using UnityEngine;
using MathDebbuger;

using CustomMath;


public class Ejercicios : MonoBehaviour
{
    [SerializeField] [Range(1, 3)] int Exercise;
    [SerializeField] float angle;

    int lastExercise = 0;

    Vec3 vec1 = new Vec3(10, 0, 0);
    Vec3 vec2 = new Vec3(10, 10, 0);
    Vec3 vec3 = new Vec3(20, 10, 0);
    Vec3 vec4 = new Vec3(20, 20, 0);

    void Start()
    {
        lastExercise = Exercise;

        Vector3Debugger.AddVector(Vector3.zero, vec1, Color.green, "V1");
        Vector3Debugger.AddVector(vec1, vec2, Color.green, "V2");
        Vector3Debugger.AddVector(vec2, vec3, Color.blue, "V3");
        Vector3Debugger.AddVector(vec3, vec4, Color.cyan, "V4");
        Vector3Debugger.EnableEditorView();

    }

    void FixedUpdate()
    {

        if (Exercise != lastExercise)
        {
            ResetVectors();
            lastExercise = Exercise;
        }

        HideVector("V1");
        HideVector("V2");
        HideVector("V3");
        HideVector("V4");


        switch (Exercise)
        {
            case 1:
                ShowVector("V1");

                vec1 = Quat.Euler(new Vec3(0, angle, 0)) * vec1;

                Vector3Debugger.UpdatePosition("V1", vec1);
                break;
            case 2:

                ShowVector("V1");
                ShowVector("V2");
                ShowVector("V3");

                vec1 = Quat.Euler(new Vec3(0, angle, 0)) * vec1;
                vec2 = Quat.Euler(new Vec3(0, angle, 0)) * vec2;
                vec3 = Quat.Euler(new Vec3(0, angle, 0)) * vec3;

                Vector3Debugger.UpdatePosition("V1", vec1);
                Vector3Debugger.UpdatePosition("V2", vec1, vec2);
                Vector3Debugger.UpdatePosition("V3", vec2, vec3);

                break;
            case 3:

                ShowVector("V1");
                ShowVector("V2");
                ShowVector("V3");
                ShowVector("V4");


                vec1 = Quat.Euler(new Vec3(angle, angle, 0)) * vec1;
                vec3 = Quat.Euler(new Vec3(-angle, -angle, 0)) * vec3;

                Vector3Debugger.UpdatePosition("V1", vec1);
                Vector3Debugger.UpdatePosition("V2", vec1, vec2);
                Vector3Debugger.UpdatePosition("V3", vec2, vec3);
                Vector3Debugger.UpdatePosition("V4", vec3, vec4);

                break;
        }
    }
    private void HideVector(string vecName)
    {
        Vector3Debugger.TurnOffVector(vecName);
        Vector3Debugger.DisableEditorView(vecName);
    }

    private void ShowVector(string vecName)
    {
        Vector3Debugger.TurnOnVector(vecName);
        Vector3Debugger.EnableEditorView(vecName);
    }

    void ResetVectors()
    {
        vec1 = new Vec3(10, 0, 0);
        vec2 = new Vec3(10, 10, 0);
        vec3 = new Vec3(20, 10, 0);
        vec4 = new Vec3(20, 20, 0);
    }
}
