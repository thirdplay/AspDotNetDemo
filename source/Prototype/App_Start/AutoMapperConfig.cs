using Prototype.Entities;
using Prototype.Models;
using Prototype.ViewModels;
using AutoMapper;
using System.Web.Mvc;

namespace Prototype
{
    /// <summary>
    /// AutoMapperの設定クラス。
    /// </summary>
    internal class AutoMapperConfig
    {
        /// <summary>
        /// マッピングを登録します。
        /// </summary>
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ListItem, SelectListItem>();
            });
        }
    }
}