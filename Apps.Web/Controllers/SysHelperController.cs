﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Apps.Common;
using Microsoft.Practices.Unity;
using Apps.BLL;
using Apps.Models.Sys;
using Apps.Web.Core;
using Apps.BLL.Sys;

namespace Apps.Web.Controllers
{
    public class SysHelperController : Controller
    {
        //
        // GET: /SysHelper/

        public SysStructBLL structBLL = new SysStructBLL();

        public SysUserBLL sysUserBLL = new SysUserBLL();

        public SysPositionBLL sysPosBLL = new SysPositionBLL();

        public ActionResult Index()
        {
            return View();
        }
        #region 上传图片
        //上传图片
        public ActionResult UpLoadImg(string id="1")
        {
            ViewBag.Dif = id;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Upload(HttpPostedFileBase fileData)
        {
            if (fileData != null)
            {
                try
                {
                    // 文件上传后的保存路径
                    string filePath = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fileName = Path.GetFileName(fileData.FileName);// 原始文件名称
                    string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                    string saveName = ResultHelper.NewId + fileExtension; // 保存文件名称

                    fileData.SaveAs(filePath + saveName);

                    return Json(new { Success = true, FileName = fileName, SaveName = saveName, FilePath = "/Uploads/"+saveName }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        //导出时候读取报表
        public ActionResult ReportControl()
        {
            return View();
        }
        //万能查询
        public ActionResult Query()
        {
            return View();
        }



        #region 获取人员选择表
        public ActionResult UserLookUp()
        {
            CommonHelper commonHelper = new CommonHelper();
            ViewBag.StructTree = structBLL.GetStructTree(true);
            return View();
        }
        public JsonResult GetUserListByDep(GridPager pager, string depId, string queryStr)
        {
            if (string.IsNullOrWhiteSpace(depId))
                return Json(0);
            var userList = sysUserBLL.m_Rep.FindList(a => a.DepId == depId).ToList();

            var jsonData = new
            {
                total = pager.totalRows,
                rows = (
                    from r in userList
                    select new SysUser()
                    {
                        Id = r.Id,
                        UserName = r.UserName,
                        TrueName = r.TrueName,
                        DepName = r.DepName,
                        PosName = r.PosName,
                        Flag = "<input type='checkbox' id='cb_" + r.Id + "' onclick='SetValue(\"" + r.Id + "\",\"" + r.TrueName + "\")'>",
                    }
                ).ToArray()
            };
            return Json(jsonData);
        }
        #endregion

        #region 获取部门选择表多选
        public ActionResult DepMulLookUp()
        {
            CommonHelper commonHelper = new CommonHelper();
            ViewBag.StructMulTree = structBLL.GetStructMulTree();
            return View();
        }
        #endregion

        #region 获取部门单选
        public ActionResult DepLookUp()
        {
            CommonHelper commonHelper = new CommonHelper();
            ViewBag.StructTree = structBLL.GetStructTree(false);
            return View();
        }
        #endregion

        #region 获取职位选择表
        public ActionResult PosMulLookUp()
        {
            CommonHelper commonHelper = new CommonHelper();
            ViewBag.StructTree = structBLL.GetStructTree(false);
            return View();
        }
        public JsonResult GetPosListByDep(GridPager pager, string depId)
        {
            var userList = sysPosBLL.m_Rep.FindList(a => a.DepId == depId).ToList();

            var jsonData = new
            {
                total = pager.totalRows,
                rows = (
                    from r in userList
                    select new SysPosition()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        DepName = r.DepName,
                        Flag = "<input type='checkbox' id='cb_" + r.Id + "' onclick='SetValue(\"" + r.Id + "\",\"" + r.Name + "\")'>",
                    }
                ).ToArray()
            };
            return Json(jsonData);
        }
        #endregion

        #region 获取职位选择表
        public ActionResult PosLookUp()
        {
            CommonHelper commonHelper = new CommonHelper();
            ViewBag.StructTree = structBLL.GetStructTree(false);
            return View();
        }
     
        #endregion
    }
}