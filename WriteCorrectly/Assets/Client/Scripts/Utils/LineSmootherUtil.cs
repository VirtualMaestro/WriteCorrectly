using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts.Utils
{
	public static class LineSmootherUtil 
	{
		public static Vector3[] SmoothLine( Vector3[] inputPoints, float segmentSize )
		{
			var curveX = new AnimationCurve();
			var curveY = new AnimationCurve();
			var curveZ = new AnimationCurve();

			var keysX = new Keyframe[inputPoints.Length];
			var keysY = new Keyframe[inputPoints.Length];
			var keysZ = new Keyframe[inputPoints.Length];

			for( var i = 0; i < inputPoints.Length; i++ )
			{
				keysX[i] = new Keyframe( i, inputPoints[i].x );
				keysY[i] = new Keyframe( i, inputPoints[i].y );
				keysZ[i] = new Keyframe( i, inputPoints[i].z );
			}

			curveX.keys = keysX;
			curveY.keys = keysY;
			curveZ.keys = keysZ;

			for( var i = 0; i < inputPoints.Length; i++ )
			{
				curveX.SmoothTangents( i, 0 );
				curveY.SmoothTangents( i, 0 );
				curveZ.SmoothTangents( i, 0 );
			}

			//list to write smoothed values to
			var lineSegments = new List<Vector3>();

			//find segments in each section
			for( var i = 0; i < inputPoints.Length; i++ )
			{
				lineSegments.Add( inputPoints[i] );

				//make sure within range of array
				if( i+1 < inputPoints.Length )
				{
					var distanceToNext = Vector3.Distance(inputPoints[i], inputPoints[i+1]);

					var segments = (int)(distanceToNext / segmentSize);

					for( var s = 1; s < segments; s++ )
					{
						//interpolated time on curve
						var time = ((float)s/(float)segments) + (float)i;

						//sample curves to find smoothed position
						var newSegment = new Vector3( curveX.Evaluate(time), curveY.Evaluate(time), curveZ.Evaluate(time) );

						lineSegments.Add( newSegment );
					}
				}
			}

			return lineSegments.ToArray();
		}
	}
}
