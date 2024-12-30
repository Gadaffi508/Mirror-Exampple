#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class AnimationWindow : EditorWindow
{
    private GameObject selectedObject;
    private AnimationClip[] animationClips;
    private bool isPlaying = false;
    private float currentAnimationTime = 0f;
    private AnimationClip currentAnimationClip;
    private double lastEditorTime;

    [MenuItem("Window/Animation Control")]
    public static void ShowWindow()
    {
        GetWindow<AnimationWindow>("Animation Control");
    }

    private void OnGUI()
    {
        GUILayout.Label("Animation Control Panel", EditorStyles.boldLabel);

        // Seçilen GameObject
        selectedObject = (GameObject)EditorGUILayout.ObjectField("GameObject", selectedObject, typeof(GameObject), true);

        if (selectedObject != null)
        {
            Animator animator = selectedObject.GetComponent<Animator>();
            Animation animationComponent = selectedObject.GetComponent<Animation>();

            if (animator != null || animationComponent != null)
            {
                if (animationClips == null || animationClips.Length == 0)
                {
                    LoadAnimationClips(animator, animationComponent);
                }

                if (animationClips != null && animationClips.Length > 0)
                {
                    foreach (var clip in animationClips)
                    {
                        if (GUILayout.Button($"Play {clip.name}"))
                        {
                            StartAnimation(clip);
                        }
                    }
                }
                else
                {
                    GUILayout.Label("No Animation Clips Found!");
                }
            }
            else
            {
                GUILayout.Label("Selected object has no Animator or Animation component.");
            }
        }
        else
        {
            GUILayout.Label("Select a GameObject to play animations.");
        }
    }

    private void LoadAnimationClips(Animator animator, Animation animationComponent)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animationClips = animator.runtimeAnimatorController.animationClips;
        }
        else if (animationComponent != null)
        {
            animationClips = new AnimationClip[animationComponent.GetClipCount()];
            int index = 0;
            foreach (AnimationState state in animationComponent)
            {
                animationClips[index] = state.clip;
                index++;
            }
        }
    }

    private void StartAnimation(AnimationClip clip)
    {
        if (selectedObject != null && clip != null)
        {
            currentAnimationClip = clip;
            currentAnimationTime = 0f;
            isPlaying = true;
            lastEditorTime = EditorApplication.timeSinceStartup;

            EditorApplication.update += UpdateAnimation;
            Debug.Log($"Started animation: {clip.name}");
        }
    }

    private void UpdateAnimation()
    {
        if (isPlaying && currentAnimationClip != null)
        {
            // Zaman kontrolü
            double currentTime = EditorApplication.timeSinceStartup;
            float deltaTime = (float)(currentTime - lastEditorTime);
            lastEditorTime = currentTime;

            // Animasyon süresini artır
            currentAnimationTime += deltaTime;

            if (currentAnimationTime > currentAnimationClip.length)
            {
                StopAnimation();
            }
            else
            {
                currentAnimationClip.SampleAnimation(selectedObject, currentAnimationTime);
                SceneView.RepaintAll(); // Sahneyi güncelle
            }
        }
    }

    private void StopAnimation()
    {
        isPlaying = false;
        currentAnimationTime = 0f;
        currentAnimationClip = null;
        EditorApplication.update -= UpdateAnimation;

        Debug.Log("Animation finished.");
    }
}
#endif
