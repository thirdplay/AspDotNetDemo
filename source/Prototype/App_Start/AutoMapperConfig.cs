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
                /*
                 * TODO: 設定内容は下記の通り。
                 * cfg.CreateMap<[コピー元の型],[コピー先の型]>();
                 */
                cfg.CreateMap<ListItem, SelectListItem>();
            });
        }
    }
}