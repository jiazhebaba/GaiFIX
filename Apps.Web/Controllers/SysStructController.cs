﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2012-12-11 22:05:42 by HD
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Apps.BLL;
using Apps.Common;
using Apps.Models;
using Apps.Models.Sys;

using System.Text;
using Apps.Web.Core;
using Apps.Locale;
using Apps.BLL.Sys;
using System;

namespace Apps.Web.Controllers
{
    public class SysStructController : BaseController
    {

        public SysStructBLL m_BLL = new SysStructBLL();
        ValidationErrors errors = new ValidationErrors();


        //////[SupportFilter]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        ////[SupportFilter(ActionName = "Index")]
        public JsonResult GetList(string id)
        {
            if (id == null)
                id = "0";
            List<SysStruct> list = m_BLL.m_Rep.FindList(a => a.ParentId == id).ToList();
            var json = from r in list
                       select new SysStruct()
                       {
                           Id = r.Id,
                           Name = r.Name,
                           ParentId = r.ParentId,
                           Sort = r.Sort,
                           Enable = r.Enable,
                           Higher = r.Higher,
                           Remark = r.Remark,
                           CreateTime = r.CreateTime,
                           state = (m_BLL.m_Rep.FindList(a => a.ParentId == r.Id.ToString()).ToList().Count > 0) ? "closed" : "open"
                       };


            return Json(json);
        }

        [HttpPost]
        public JsonResult GetListByComTree(string id)
        {
            if (id == "root")
            {
                id = "0";
            }
            List<SysStruct> list = m_BLL.m_Rep.FindList(a => a.ParentId == id).ToList();
            var json = from r in list
                       select new
                       {
                           id = r.Id.ToString(),
                           text = r.Name,
                           state = (m_BLL.m_Rep.FindList(a => a.ParentId == r.Id.ToString()).ToList().Count > 0) ? "closed" : "open"
                       };
            return Json(json);
        }

        [HttpPost]
        public JsonResult GetListByParentId(string id)
        {
            if (id == null)
                id = "0";
            List<SysStruct> list = m_BLL.m_Rep.FindList(a => a.ParentId == id).ToList();
            StringBuilder sb = new StringBuilder("");
            foreach (var i in list)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", i.Id, i.Name);
            }

            return Json(sb.ToString());
        }

        #region 创建
        //////[SupportFilter]
        public ActionResult Create(string id)
        {

            SysStruct entity = new SysStruct()
            {
                ParentId = id,
                Enable = "true"
            };
            return View(entity);
        }

        [HttpPost]
        //////[SupportFilter]
        public JsonResult Create(SysStruct model)
        {
            model.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd");
            if (model != null)
            {

                if (m_BLL.m_Rep.Create(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "创建", "SysStruct");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "创建", "SysStruct");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail));
            }
        }
        #endregion

        #region 修改
        //////[SupportFilter]
        public ActionResult Edit(string id)
        {

            SysStruct entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            return View(entity);
        }

        [HttpPost]
        //////[SupportFilter]
        public JsonResult Edit(SysStruct model)
        {
            if (model != null)
            {

                if (m_BLL.m_Rep.Update(model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "修改", "SysStruct");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "修改", "SysStruct");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":" + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }
        }
        #endregion

        #region 详细

        public ActionResult Details(string id)
        {
            //获取父级
            List<SysStruct> list = m_BLL.m_Rep.FindList(a => a.ParentId == "0").ToList();

            foreach (var model in list)
            {
                model.clildren = m_BLL.m_Rep.FindList(a => a.ParentId == model.Id.ToString()).ToList();
                foreach (var m in model.clildren)
                {
                    m.clildren = m_BLL.m_Rep.FindList(a => a.ParentId == m.Id.ToString()).ToList();
                }
            }

            return View(list);
        }

        #endregion

        #region 删除
        [HttpPost]
        //////[SupportFilter]
        public JsonResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (m_BLL.m_Rep.Delete(Convert.ToInt32(id))>0)
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "SysStruct");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "SysStruct");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail));
            }


        }
        #endregion

        [HttpPost]
        public ActionResult departmentJsonList()
        {
            return Json(m_BLL.departmentComboxData());
        }
    }
}