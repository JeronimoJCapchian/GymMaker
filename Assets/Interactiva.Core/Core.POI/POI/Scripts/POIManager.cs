using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Interactiva.Core.Navigation;

namespace Interactiva.Core.POIs
{
    public class POIManager : MonoBehaviour
    {
        public static POIManager singleton; //POI manager will only have one instance of itself

        private List<POI> POIs = new List<POI>();

        private void Start()
        {
            if (singleton == null)
            {
                singleton = this;
            } else
            {
                Debug.LogError("There is more than one singleton for the POIManager object.");
                return;
            }
        }
        

        /// <summary>
        /// Method gets the best point of interest, if any, within the thresholds given
        /// --Used to calculate which point of interest the player can zoom into
        /// </summary>
        /// <param name="cameraPos">The transform of the navigator's camera</param>
        /// <param name="dist">The distance threshold for a POI to be focusable</param>
        /// <param name="angle">The angle threshold for a POI to be focusable (from camera's perspective)</param>
        /// <returns>POI within given thresholds of camera, if no POI then returns null</returns>
        public POI GetBestFocusablePOI(Transform cameraPos, float dist, float angle)
        {
            POI bestPOI = null;

            if (!POIs.Any()) return bestPOI;

            if (POIs.All(p => !p.LookAtPivot)) return POIs.Aggregate(POIs[0], (x, y) =>
            {
                return Vector3.Distance(y.transform.position, FocusComponent.singleton.transform.position) > Vector3.Distance(x.transform.position, FocusComponent.singleton.transform.position) ? x : y;
            });
            else if (POIs.Any(p => p.LookAtPivot))
            {
                float bestAngle = 360f;
                for (int i = 0; i < POIs.Count; i++)
                {
                    if (Vector3.Distance(cameraPos.position, POIs[i].GetPosition()) <= dist)
                    {
                        float thisAngle = Vector3.Angle(Vector3.forward, cameraPos.InverseTransformPoint(POIs[i].GetPosition()));
                        if (thisAngle <= angle)
                        {
                            if (thisAngle < bestAngle)
                            {
                                bestAngle = thisAngle;
                                bestPOI = POIs[i];
                            }
                        }
                    }
                }
            }

            return bestPOI;
        }

        /// <summary>
        /// Method adds a focusable POI to the list, so that we can focus it.
        /// Usually executed by a trigger.
        /// </summary>
        /// <param name="poi"></param>
        public static void AddFocusablePOI(POI poi)
        {
            if (!singleton.POIs.Contains(poi))
            {
                singleton.POIs.Add(poi);
            }
        }

        public static void RemoveFocusablePOI(POI poi)
        {
            if (singleton.POIs.Contains(poi))
            {
                singleton.POIs.Remove(poi);
            }
        }

        
        /// <summary>
        /// The number of POIs in given group
        /// </summary>
        /// <param name="groupIndex">The group with the POIs.</param>
        /// <returns>The number of POIs</returns>
        public int GetPOICount()
        {
            return POIs.Count;
        }
    }
}
