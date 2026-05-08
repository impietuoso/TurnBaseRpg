using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

namespace Drafts
{
    public class CollectionView : DataView<IEnumerable>
    {
        [SerializeField] private DataView itemTemplate;
        [SerializeField] private List<DataView> views = new();

        public IReadOnlyList<DataView> Views => views;

        private bool? _isFixed2;
        private bool IsFixed => _isFixed2 ??= views.Count > 0;

        protected virtual void Awake()
        {
            if (itemTemplate)
                itemTemplate.gameObject.SetActive(false);
        }

        protected override void Subscribe()
        {
            if (Data is INotifyCollectionChanged notifyCollection)
                notifyCollection.CollectionChanged += CollectionChanged;

            if (IsFixed)
            {
                var items = Data.GetEnumerator();
                for (var i = 0; i < views.Count; i++)
                {
                    var view = views[i];
                    view.SetData(items.MoveNext() ? items.Current : null);
                    //view.GetComponent<IntView>()?.SetData(i);
                }

                if (items is IDisposable d) d.Dispose();
            }
            else
            {
                var index = 0;
                foreach (var item in Data)
                    AddItem(index++, item);
            }
        }

        protected override void Unsubscribe()
        {
            if (Data is INotifyCollectionChanged notifyCollection)
                notifyCollection.CollectionChanged -= CollectionChanged;
            
            if (IsFixed)
            {
                foreach (var view in views)
                    view.SetData(null);
            }
            else
            {
                foreach (var view in views)
                    Destroy(view.gameObject);
                views.Clear();
            }
        }

        public void SetItem(int index, object data)
        {
            //TODO shift items
            views[index].SetData(data);
        }

        public void AddItem(int index, object item)
        {
            var view = Instantiate(itemTemplate, itemTemplate.transform.parent);
            view.transform.SetSiblingIndex(index + 1); // 0 is the template
            view.gameObject.SetActive(true);

            view.SetData(item);
            views.Insert(index, view);
        }

        public void RemoveItem(int index)
        {
            Destroy(views[index].gameObject);
            views.RemoveAt(index);
        }

        private void RemoveItems(IList items)
        {
            foreach (var data in items)
                for (var i = views.Count - 1; i >= 0; i--)
                    if (views[i].GetData() == data)
                        RemoveItem(i);
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var index = e.NewStartingIndex < 0
                        ? itemTemplate.transform.childCount
                        : e.NewStartingIndex;

                    for (var i = 0; i < e.NewItems.Count; i++)
                        if (IsFixed) SetItem(index + i, e.NewItems[i]);
                        else AddItem(index + i, e.NewItems[i]);
                    break;

                case NotifyCollectionChangedAction.Move:
                    Debug.LogError("Move not implemented");
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 0)
                        RemoveItems(e.OldItems);
                    else
                        for (var i = 0; i < e.OldItems.Count; i++)
                            RemoveItem(e.OldStartingIndex + i);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    for (var i = 0; i < e.OldItems.Count; i++)
                        views[e.OldStartingIndex + i].SetData(e.NewItems[i]);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    if (IsFixed)
                        foreach (var view in views)
                            view.SetData(null);
                    else
                    {
                        foreach (var view in views) Destroy(view.gameObject);
                        views.Clear();
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}