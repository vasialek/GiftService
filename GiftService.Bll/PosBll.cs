using GiftService.Dal;
using GiftService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public interface IPosBll
    {
        PosBdo GetById(int posId);
        int GetPosIdFromUid(string posUid);
    }

    public class PosBll : IPosBll
    {
        private IConfigurationBll _configurationBll = null;
        private IPosDal _posDal = null;

        public PosBll(IConfigurationBll configurationBll, IPosDal posDal)
        {
            if (configurationBll == null)
            {
                throw new ArgumentNullException("configurationBll");
            }
            if (posDal == null)
            {
                throw new ArgumentNullException("posDal");
            }

            _configurationBll = configurationBll;
            _posDal = posDal;
        }

        public PosBdo GetById(int posId)
        {
            return _posDal.GetById(posId);
        }

        public int GetPosIdFromUid(string posUid)
        {
            int posId = -1;

            if (posUid.Length != _configurationBll.Get().LengthOfPosUid)
            {
                throw new ArgumentException("posUid", "Length of UID from POS must be " + _configurationBll.Get().LengthOfPosUid);
            }

            string s = String.Concat(posUid[1], posUid[4], posUid[7], posUid[10]);
            if (int.TryParse(s, out posId) == false)
            {
                throw new ArgumentException("Incorrect ID of POS (from UID): " + s);
            }

            if (posId < 1)
            {
                throw new ArgumentOutOfRangeException("Incorrect POS ID from UID");
            }

            return posId;
        }
    }
}
