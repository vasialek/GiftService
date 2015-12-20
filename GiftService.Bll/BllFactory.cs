﻿using GiftService.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public class BllFactory
    {
        private static BllFactory _bllFactory = null;

        private IConfigurationBll _configurationBll = null;
        private ISecurityBll _securityBll = null;
        private ICommunicationBll _communicationBll = null;
        private ITransactionsBll _transactionsBll = null;
        private IPosBll _posBll = null;
        private IProductsBll _productsBll = null;
        private PayseraBll _payseraBll = null;
        private IPdfBll _pdfBll = null;
        private IHelperBll _helperBll = null;
        private ILogsBll _logsBll = null;

        private BllFactory()
        {
        }

        public static BllFactory Current
        {
            get
            {
                if (_bllFactory == null)
                {
                    _bllFactory = new BllFactory();
                }
                return _bllFactory;
            }
        }


        public IConfigurationBll ConfigurationBll
        {
            get
            {
                if (_configurationBll == null)
                {
                    _configurationBll = new ConfigurationBll();
                }
                return _configurationBll;
            }
        }

        public ISecurityBll SecurityBll
        {
            get
            {
                if (_securityBll == null)
                {
                    _securityBll = new SecurityBll(ConfigurationBll, CommunicationBll);
                }
                return _securityBll;
            }
        }


        public ICommunicationBll CommunicationBll
        {
            get
            {
                if (_communicationBll == null)
                {
                    _communicationBll = new CommunicationBll();
                }
                return _communicationBll;
            }
        }

        public IProductsBll GiftsBll
        {
            get
            {
                if (_productsBll == null)
                {
                    _productsBll = new ProductsBll(DalFactory.Current.ProductsDal, DalFactory.Current.PosDal);
                }
                return _productsBll;
            }
        }

        public IPosBll PosBll
        {
            get
            {
                if (_posBll == null)
                {
                    _posBll = new PosBll(ConfigurationBll, DalFactory.Current.PosDal);
                }
                return _posBll;
            }
        }

        public ITransactionsBll TransactionsBll
        {
            get
            {
                if (_transactionsBll == null)
                {
                    _transactionsBll = new TransactionsBll(ConfigurationBll, SecurityBll, DalFactory.Current.TransactionDal);
                }
                return _transactionsBll;
            }
        }


        public PayseraBll PayseraBll
        {
            get
            {
                if (_payseraBll == null)
                {
                    _payseraBll = new PayseraBll();
                }
                return _payseraBll;
            }
        }

        public IPdfBll PdfBll
        {
            get
            {
                if (_pdfBll == null)
                {
                    _pdfBll = new PdfBll(ConfigurationBll, DalFactory.Current.ProductsDal);
                }
                return _pdfBll;
            }
        }


        public IHelperBll HelperBll
        {
            get
            {
                if (_helperBll == null)
                {
                    _helperBll = new HelperBll();
                }
                return _helperBll;
            }
        }

        public ILogsBll LogsBll
        {
            get
            {
                if (_logsBll == null)
                {
                    _logsBll = new LogsBll(DalFactory.Current.LogsDal);
                }
                return _logsBll;
            }
        }

    }
}
