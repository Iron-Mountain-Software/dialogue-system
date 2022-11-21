using System.Collections;
using UnityEngine;

namespace ARISE.DialogueSystem.DialogueBubbles
{
    [RequireComponent(typeof(DialogueBubble))]
    public class DialogueBubbleAnimator : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Vector3 scaleMax;
        [SerializeField] private Vector3 scaleMin;
        [SerializeField] private float defaultDuration;

        public void ScaleUp() => ScaleUp(defaultDuration);
        
        public void ScaleUp(float duration)
        {
            StopAllCoroutines();
            if (!gameObject.activeInHierarchy) return;
            StartCoroutine(TransitionScale(scaleMin, scaleMax, duration));
        }

        public void ScaleUpImmediate()
        {
            StopAllCoroutines();
            transform.localScale = scaleMax;
        }

        public void ScaleDown() => ScaleDown(defaultDuration);
        
        public void ScaleDown(float duration)
        {
            StopAllCoroutines();
            if (!gameObject.activeInHierarchy) return;
            StartCoroutine(TransitionScale(scaleMax, scaleMin, duration));
        }
        
        public void ScaleDownImmediate()
        {
            StopAllCoroutines();
            transform.localScale = scaleMin;
        }

        private IEnumerator TransitionScale(Vector3 startScale, Vector3 endScale, float duration)
        {
            float progress = Mathf.InverseLerp(startScale.x, endScale.x, transform.localScale.x);
            for (float timer = progress * duration; timer < duration; timer += Time.unscaledDeltaTime)
            {
                progress = timer / duration;
                transform.localScale = Vector3.Lerp(startScale, endScale, progress);
                yield return null;
            }
            transform.localScale = endScale;
        }
    }
}