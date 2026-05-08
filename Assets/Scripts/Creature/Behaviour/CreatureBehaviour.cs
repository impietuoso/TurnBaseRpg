using System.Collections;
using UnityEngine;

namespace TricksAndTreatsOrThreats.Behaviour
{
    public class CreatureBehaviour : DataView<Creature>
    {
        [SerializeField] private Stat speedStat;

        public Animator Animator { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }

        public Coroutine _moveCoroutine;

        private void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
        }

        protected override void Subscribe()
        {
            Animator = Instantiate(Data.race.Model, transform);
        }

        protected override void Unsubscribe()
        {
            if (Animator) Destroy(Animator.gameObject);
        }

        public void MoveTo(Vector3 position)
        {
            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
            _moveCoroutine = StartCoroutine(MoveRoutine(position));
        }

        private IEnumerator MoveRoutine(Vector3 position)
        {
            if (Vector3.Distance(transform.position, position) <= 0.01f) 
                yield break;
            
            if (!Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                Animator.Play("Walk");

            //var speed = 1 + Data.stats[speedStat] * 0.2f;
            var speed = 2;

            while (Vector3.Distance(transform.position, position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
                yield return null;
            }

            transform.position = position;
            Animator.Play("Idle");
            _moveCoroutine = null;
        }
    }
}