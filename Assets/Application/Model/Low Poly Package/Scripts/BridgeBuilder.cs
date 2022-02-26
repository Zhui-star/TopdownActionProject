using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BridgeBuilder : MonoBehaviour 
{
	public int boardsCount;
	public GameObject boardPrefab1;
	public GameObject boardPrefab2;

	private List<GameObject> boards;

    /// <summary>
    /// 生成桥的脚本
    /// </summary>
	public void BuildBridge()
	{
		deleteBridge();
		spawnBoards();
		attachBoardComponents();
		configureJoints();
	}

    /// <summary>
    /// 删除所有子物体
    /// </summary>
	public void deleteBridge(){
		Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>(true);
		foreach (Transform child in ts) {
			if (gameObject.name != child.name) {
				DestroyImmediate(child.gameObject);
			}
		}
		if (boards != null) {
			boards.Clear();
		}
	}

    /// <summary>
    /// 生成木板
    /// 各2.2米生成一个木板
    /// </summary>
	private void spawnBoards(){
		boards = new List<GameObject>();
		Vector3 spawnPos = transform.position;
		bool invertRot = false;
		for(int i = 0; i < boardsCount; i++) {
			if (!invertRot) {
				boards.Add((GameObject)Instantiate(boardPrefab1, spawnPos, boardPrefab1.transform.rotation));
			} else {
				boards.Add((GameObject)Instantiate(boardPrefab2, spawnPos, boardPrefab2.transform.rotation));
			}
			boards[i].transform.parent = gameObject.transform;
			boards[i].name += i;
			spawnPos.x += 2.2f;
			invertRot = !invertRot;
		}
	}

    /// <summary>
    /// 给木板附上组件 rigidbody collider joint
    /// </summary>
	private void attachBoardComponents(){
		for (int i = 0; i < boards.Count; i++) {
			attachCollider(boards[i]);
			attachRigidbody(boards[i]);
			attachJoints(boards[i]);
		}
	}

	private void attachRigidbody(GameObject board){
		if (board.GetComponent<Rigidbody>() == null) {
			board.AddComponent<Rigidbody>();
		}
	}

	private void attachCollider(GameObject board){
		if (board.GetComponent<MeshCollider>() == null) {
			board.AddComponent<MeshCollider>();
			board.GetComponent<MeshCollider>().convex = true;
		}
	}

	private void attachJoints(GameObject board){
		HingeJoint[] joints = board.GetComponents<HingeJoint>();
		if (joints.Length < 2) {
			for(int i = 0; i < 2 - joints.Length; i++) {
				board.AddComponent<HingeJoint>();
			}
		}
	}

    /// <summary>
    /// 配置HineJoint
    /// </summary>
	private void configureJoints(){
		for (int position = 0; position < boards.Count; position++) {
			HingeJoint[] joints = boards[position].GetComponents<HingeJoint>();
			if (joints.Length == 2) {
				Vector3 axis = new Vector3(0,1,0);
				configureJoint(position, -1, joints[0], new Vector3(-0.011f, 0,0), axis);
				configureJoint(position, 1, joints[1], new Vector3(0.011f, 0,0), axis);
			}
		}
	}

	private void configureJoint(int boardPos, int shift, HingeJoint joint, Vector3 anchor, Vector3 axis) {
		joint.anchor = anchor;
		joint.axis = axis;
		joint.useLimits = true;

        //限制
		JointLimits jl = new JointLimits();
		jl.min = -15f;
		jl.max = 15f;
		jl.bounceMinVelocity = 0f;
		joint.limits = jl;
		int connectedRigidbodyPos = boardPos + shift; 
		if ((connectedRigidbodyPos < boards.Count) && (connectedRigidbodyPos >= 0 )){
			joint.connectedBody = boards[connectedRigidbodyPos].GetComponent<Rigidbody>();
		} else {
			joint.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			DestroyImmediate(joint);
		}
	}

}