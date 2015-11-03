using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IPointCloud  {

	IEnumerable<Vector3> GetPoints();
	void SetPoints(IEnumerable<Vector3> points);

}
