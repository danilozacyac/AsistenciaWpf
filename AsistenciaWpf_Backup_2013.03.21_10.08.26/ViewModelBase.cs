

namespace AsistenciaWpf
{
    /// <summary>
    /// Base class for all ViewModel classes.
    /// It provides support for property change notifications and has a DisplayName property.
    /// This class is abstract.
    /// </summary>
    public abstract class ViewModelBase : System.Object, System.ComponentModel.INotifyPropertyChanged, System.IDisposable
    {
        /*
        private System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Telerik.Windows.Controls.ViewModelBase"/>
        /// class.
        /// </summary>
        protected ViewModelBase()
        {
        }

        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This 
        /// method does not exist in a Release build.
        /// </summary>
        [System.Diagnostics.ConditionalAttribute(@"DEBUG")]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        protected void VerifyPropertyName(string propertyName)
        {
            this.GetType().GetProperty(propertyName) == null;
        }

        /// <summary>
        /// Invokes the specified action on the UI thread.
        /// </summary>
        /// <param name="action">An Action to be invoked on the UI thread.</param>
        public static void InvokeOnUIThread(System.Action action)
        {
            System.Windows.Threading.Dispatcher currentDispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
            if (!currentDispatcher.CheckAccess())
            {
                currentDispatcher.BeginInvoke(action, new object[0]);
                return;
            }
            else
            {
                action();
                return;
            }
        }

        /// <summary>
        /// Raises this object's <see cref="E:Telerik.Windows.Controls.ViewModelBase.PropertyChanged"/>
        /// event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
            if (propertyChangedEventHandler != null)
            {
                System.ComponentModel.PropertyChangedEventArgs propertyChangedEventArg = new System.ComponentModel.PropertyChangedEventArgs(propertyName);
                propertyChangedEventHandler(this, propertyChangedEventArg);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute(@"Microsoft.Design", @"CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected virtual void OnPropertyChanged<T>(System.Linq.Expressions.Expression<System.Func<T>> propertyExpression)
        {
            this.OnPropertyChanged(((System.Linq.Expressions.MemberExpression)propertyExpression.Body).Member.Name);
        }

        internal void RaisePropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(propertyName);
        }
        */
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            this.Dispose(true);
            System.GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }
        

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
