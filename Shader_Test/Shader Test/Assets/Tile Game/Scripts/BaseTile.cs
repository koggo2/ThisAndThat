using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheTile.Game
{
    public class BaseTile : BaseObject
    {
        public Vector3Int CellPos;

        public Transform Pivot => _pivot;
        [SerializeField] private Transform _pivot;

        private GameObject _selectionUI = null;
        
        [ExecuteInEditMode]
        private void Awake()
        {
            _pivot.localPosition = new Vector3(0f, Random.Range(-1f, 1f), 0f);
        }
        
        public void SetTeam(TeamEnum unitTeam)
        {
            Team = unitTeam;
            
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                renderer.material.color = Const.GetTeamColor(Team);
            }
        }

        public void AttachUnit(BaseUnit unit)
        {
            unit.transform.parent = _pivot.transform;
            unit.transform.localPosition = Vector3.zero;
        }

        private void OnMouseEnter()
        {
            if (_selectionUI != null)
            {
                DestroyImmediate(_selectionUI);                
            }
            
            var selectionUIPrefab = Resources.Load<GameObject>("Tile Selection");
            var instance = Instantiate(selectionUIPrefab);
            instance.transform.parent = _pivot;
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;

            _selectionUI = instance;
        }

        private void OnMouseUp()
        {
            
        }

        private void OnMouseExit()
        {
            if (_selectionUI != null)
            {
                DestroyImmediate(_selectionUI);                
            }
        }
    }
}
