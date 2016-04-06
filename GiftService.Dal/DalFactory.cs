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
        private ITransactionDal _transactionDal = null;
        private ILogsDal _logsDal = null;
        private ITextModuleDal _textModuleDal = null;

        private DalFactory()
        {
        }

        public static DalFactory Current
        {
            get
            {
                if (_dalFactory == null)
                {
                    AutoMapperConfigDal.RegisterMapping();
                    _dalFactory = new DalFactory();
                }
                return _dalFactory;
            }
        }

        public ITransactionDal TransactionDal
        {
            get
            {
                if (_transactionDal == null)
                {
                    _transactionDal = new TransactionDal();
                }
                return _transactionDal;
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

        public ILogsDal LogsDal
        {
            get
            {
                if (_logsDal == null)
                {
                    _logsDal = new LogsDal();
                }
                return _logsDal;
            }
        }

        public ITextModuleDal TextModuleDal
        {
            get
            {
                if (_textModuleDal == null)
                {
                    _textModuleDal = new TextModuleDal();
                }
                return _textModuleDal;
            }
        }
    }
}
