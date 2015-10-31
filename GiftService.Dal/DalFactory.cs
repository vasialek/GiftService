using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Dal
{
    public class DalFactory
    {
        private static DalFactory _dalFactory = null;

        private IPosDal _posDal = null;
        private IProductsDal _productsDal = null;

        private DalFactory()
        {
        }

        public static DalFactory Current
        {
            get
            {
                if (_dalFactory == null)
                {
                    _dalFactory = new DalFactory();
                }
                return _dalFactory;
            }
        }


        public IPosDal PosDal
        {
            get
            {
                if (_posDal == null)
                {
                    _posDal = new PosDal();
                }
                return _posDal;
            }
        }

        public IProductsDal ProductsDal
        {
            get
            {
                if (_productsDal == null)
                {
                    _productsDal = new ProductsDal();
                }
                return _productsDal;
            }
        }

    }
}
