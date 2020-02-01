using UnityEngine;

namespace AsagiHandyScripts
{
    public static class RaycastFromTouchPosition
    {
        public static RaycastHit? GetRaycast(Vector3 touchPos, LayerMask target)
        {
            return GetRaycast(Camera.main, touchPos, target);
        }

        public static RaycastHit? GetRaycast(Camera camera, Vector3 touchPos, LayerMask target)
        {
            Ray ray = camera.ScreenPointToRay(touchPos);
            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(ray, out hitInfo, 100, target))
            {
                return hitInfo;
            }
            return null;
        }
    }
}
