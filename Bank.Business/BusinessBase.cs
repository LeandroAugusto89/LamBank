using Bank.Data;

namespace Bank.Business
{
    public class BusinessBase
    {
        private BDContext _bdcontext;

        public BDContext BDContext { get { return this._bdcontext; } }

        public BusinessBase()
        {
            _bdcontext = new BDContext();
        }
    }
}