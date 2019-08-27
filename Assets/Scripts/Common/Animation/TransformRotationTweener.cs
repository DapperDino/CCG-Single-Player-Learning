namespace TheLiquidFire.Animation
{
    public class TransformRotationTweener : QuaternionTweener
    {
        protected override void OnUpdate()
        {
            base.OnUpdate();
            transform.rotation = currentTweenValue;
        }
    }
}