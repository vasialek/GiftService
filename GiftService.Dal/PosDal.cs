using GiftService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Dal
{
    public interface IPosDal
    {
        PosBdo GetById(int posId);
    }

    public class PosDal : IPosDal
    {
        private List<PosBdo> _poses = new List<PosBdo>();

        public PosDal()
        {
            PosBdo p = new PosBdo();
            p.Id = 1005;
            p.Name = "Ritos Masazai";
            p.PosUrl = new Uri("http://www.ritosmasazai.lt/");
            p.ValidateUrl = new Uri("http://localhost:56620/Test/Validate");
            _poses.Add(p);
        }

        public PosBdo GetById(int posId)
        {
            return _poses.First(x => x.Id == posId);
        }
    }
}
