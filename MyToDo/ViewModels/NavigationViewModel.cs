using MyToDo.Extension;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MyToDo.ViewModels
{
    public class NavigationViewModel : BindableBase,INavigationAware
    {
        private readonly IContainerProvider provider;
        public readonly IEventAggregator aggregator;

        public NavigationViewModel(IContainerProvider provider) 
        {
            this.provider = provider;
            aggregator = provider.Resolve<IEventAggregator>();
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }
        public void UpdateLoading(bool IsOpen) 
        {
            aggregator.UpdateLoading(new Common.Events.UpdateModel()
            {
                IsOpen=IsOpen
            });
        }

    }
}
