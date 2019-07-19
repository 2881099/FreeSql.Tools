using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace Wpf.FreeSqlTools
{

 
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
     
  
        private SynchronizationContext _synchronizationContext;
        SynchronizationContext SynchronizationContext
        {
            get
            {
                if (_synchronizationContext == null)
                {
                    _synchronizationContext = SynchronizationContext.Current;
                }
                return _synchronizationContext;
            }
        }
        public AsyncObservableCollection() { }
        public AsyncObservableCollection(IEnumerable<T> list) : base(list) { }
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {

            if (SynchronizationContext.Current == SynchronizationContext)
            {
                //如果操作发生在同一个线程中，不需要进行跨线程执行         
                RaiseCollectionChanged(e);
            }
            else
            {
                //如果不是发生在同一个线程中
                //准确说来，这里是在一个非UI线程中，需要进行UI的更新所进行的操作         
                SynchronizationContext.Post(RaiseCollectionChanged, e);
            }
        }
        private void RaiseCollectionChanged(object param)
        {
            // 执行         
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {

            if (SynchronizationContext.Current == SynchronizationContext)
            {
                   
                RaisePropertyChanged(e);
            }
            else
            {
                         
                SynchronizationContext.Post(RaisePropertyChanged, e);
            }
        }
        private void RaisePropertyChanged(object param)
        {
             
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }
    }
}
