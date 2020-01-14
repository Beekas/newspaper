using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newspaper.Models;
using Newspaper.Models.DAL;
using Newspaper.Models.ViewModel;
using Newspaper.Filters;
using System.Collections;

namespace Newspaper.Controllers
{
    [SessionCheck(Role = "SuperAdmin,Admin,Supervisor,Counter")]
    [ValidateInput(false)]
    public class CounterController : Controller
    {
        private activities log = new activities();


        private NewspaperEntities db = new NewspaperEntities();

        // GET: Counter
        public ActionResult Index()
        {
            var cus = (from s in db.ServiceAssign
                       from c in db.Customer
                       from n in db.Service
                       where s.NewspaperId == n.Id && s.CustomerId == c.Id
                       select new
                       {
                           Customer = c,
                           ServiceAssign = s,
                           NewsPaper = n
                       }).ToList(); // as IEnumerable<CounterVM>;
            var uniqCustomer = from m in cus
                               group m by new { m.Customer.Id }
                               into mygroup
                               select mygroup.FirstOrDefault();


            List<CounterVM> objConter = new List<CounterVM>();


            foreach (var item in cus)
            {
                CounterVM counter = new CounterVM();
                counter.NewsPaper = item.NewsPaper;
                counter.Customer = item.Customer;
                counter.ServiceAssign = item.ServiceAssign;
                objConter.Add(counter);

            }
            return View(objConter.AsEnumerable());

        }

        public ActionResult EditEndedDate(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //ServiceAssign service = db.ServiceAssign.Find(id);
            //if (service == null)
            //{
            //    return HttpNotFound();
            //}


            //return View(service);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cus = (from s in db.ServiceAssign
                       from c in db.Customer
                       from n in db.Service
                       
                       where s.NewspaperId == n.Id && s.CustomerId == c.Id && s.Id == id
                       select new
                       {
                           Customer = c,
                           ServiceAssign = s,
                           NewsPaper = n
                       }); // as IEnumerable<CounterVM>;



            CounterVM counter = new CounterVM();

            if (cus.ToList().Any())
            {
                counter.NewsPaper = cus.ToList().First().NewsPaper;
                
                counter.Customer = cus.ToList().First().Customer;
                counter.ServiceAssign = cus.ToList().First().ServiceAssign;
                counter.payment = counter.ServiceAssign.PaymentType;
                counter.Billdate = ADTOBS.EngToNep(counter.ServiceAssign.UpdatedDate).ToString();
                counter.Paperdispatch = ADTOBS.EngToNep(counter.ServiceAssign.PaperDispatchDate).ToString();
                counter.enddate = ADTOBS.EngToNep(counter.ServiceAssign.EndedDate).ToString();
                
            }
         
            return View(counter);

        }
        [HttpPost]
        public ActionResult EditEndedDate(DateTime PaperDispatchDate,DateTime EndedDate,bool payment,int Quantity,int id,DateTime PaymentDate)
        {
            ServiceAssign objserviceAssign = db.ServiceAssign.Find(id);
            objserviceAssign.PaperDispatchDate = PaperDispatchDate;
            objserviceAssign.EndedDate = EndedDate;
            objserviceAssign.UpdatedDate = PaymentDate;
            objserviceAssign.Quantity = Quantity;
            objserviceAssign.PaymentType = payment;
            db.SaveChanges();
            TempData["message"] = "EndedDate Edited Sucessfully";
            log.AddActivity("EndedDate is changed");
            return RedirectToAction("Index","Counter");
        }



        public ActionResult SelectToEditDate()
        {
            var cus = (from s in db.ServiceAssign
                       from c in db.Customer
                       from n in db.Service
                       where s.NewspaperId == n.Id && s.CustomerId == c.Id
                       select new
                       {
                           Customer = c,
                           ServiceAssign = s,
                           NewsPaper = n
                       }).ToList(); // as IEnumerable<CounterVM>;
            var uniqCustomer = from m in cus
                               group m by new { m.Customer.Id }
                               into mygroup
                               select mygroup.FirstOrDefault();


            List<CounterVM> objConter = new List<CounterVM>();


            foreach (var item in cus)
            {
                CounterVM counter = new CounterVM();
                counter.NewsPaper = item.NewsPaper;
                counter.Customer = item.Customer;
                counter.ServiceAssign = item.ServiceAssign;
                objConter.Add(counter);

            }
            return View(objConter.AsEnumerable());

        }





        // GET: Counter/Details/5
        public ActionResult CustomerDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cus = (from s in db.ServiceAssign
                       from c in db.Customer
                       from n in db.Service
                       where s.NewspaperId == n.Id && s.CustomerId == c.Id && s.CustomerId == id
                       select new
                       {
                           Customer = c,
                           ServiceAssign = s,
                           NewsPaper = n
                       }).ToList(); // as IEnumerable<CounterVM>;



            List<CounterVM> objConter = new List<CounterVM>();


            foreach (var item in cus)
            {
                CounterVM counter = new CounterVM();
                counter.NewsPaper = item.NewsPaper;
                counter.Customer = item.Customer;
                counter.ServiceAssign = item.ServiceAssign;
                objConter.Add(counter);

            }


            if (objConter.AsEnumerable() == null)
            {
                return HttpNotFound();
            }
            return View(objConter.AsEnumerable());
        }



        public ActionResult CustomerActivate(int? id)
        {
            FiscalYear fiscalYear = db.FiscalYear.FirstOrDefault(m=>m.Status==true);
            int billNo = 1;

            if (fiscalYear != null)
            {
                if (db.Account.Any(m => m.fiscalyear.Id == fiscalYear.Id))
                {
                    billNo = db.Account.Where(m => m.fiscalyear.Id == fiscalYear.Id).Max(m => m.BillNo) + 1;
                }
            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list = new List<SelectListItem>
                            {new SelectListItem{ Text = "--छान्नुहोस्-- ", Value = "" },
                            new SelectListItem{ Text = "मासिक", Value = "30" },
                            new SelectListItem{ Text = "त्रेमासिक", Value = "90" },
                            new SelectListItem{ Text = "चौ मासिक", Value = "120" },
                            new SelectListItem { Text = "अर्ध  बार्षिक", Value = "182" },
                                        new SelectListItem{ Text = "बार्षिक", Value = "365" },
                                        new SelectListItem{ Text = "२ वर्ष", Value = "730" },
                                        new SelectListItem{ Text = "३ वर्ष", Value = "1095" },
                                        new SelectListItem{ Text = "४ वर्ष", Value = "1460" },
                                        new SelectListItem{ Text = "५ वर्ष", Value = "1825" }
                            };
            // SelectList countries = new SelectList({ Text = "Quarterly", Value = "90" }, new SelectListItem() { Text = "Half Quarterly", Value = "183" }, new SelectListItem() { Text = "Yearly", Value = "365" } }


            var cus = (from s in db.ServiceAssign
                       from c in db.Customer
                       from n in db.Service
                       where s.NewspaperId == n.Id && s.CustomerId == c.Id && s.Id == id
                       select new
                       {
                           Customer = c,
                           ServiceAssign = s,
                           NewsPaper = n
                       }); // as IEnumerable<CounterVM>;



            CounterVM counter = new CounterVM();

            if (cus.ToList().Any())
            {
                counter.NewsPaper = cus.ToList().First().NewsPaper;
                counter.Customer = cus.ToList().First().Customer;
                counter.ServiceAssign = cus.ToList().First().ServiceAssign;
                counter.Paperdispatch = ADTOBS.EngToNep(counter.ServiceAssign.PaperDispatchDate).ToString();
                counter.enddate = ADTOBS.EngToNep(counter.ServiceAssign.EndedDate).ToString();
                counter.fiscalYear = fiscalYear.NepYear;
                counter.billNo = billNo;
               
            }
            ViewBag.Duration = new SelectList(list, "Value", "Text", cus.ToList().FirstOrDefault().ServiceAssign.Duration);
            return View(counter);

        }

        [HttpPost]
        public ActionResult CustomerActivate(string BillNo, string PaymentDate,string Duration,string NepaliDate1,DateTime PaperDispatchDate, string Nepalidate, string PaymentType, string BankName, string ACNumber, decimal Amount,bool payment, decimal Discount, int id)
        {
            ServiceAssign objserviceAssign = db.ServiceAssign.Find(id);
            //int quant = objserviceAssign.Quantity;

            //if (objserviceAssign.BillNo != null && objserviceAssign.EndedDate < PaperDispatchDate)
            //{
            objserviceAssign.Duration = Duration;
            //}
            objserviceAssign.NepaliDate = NepaliDate1;

           
            objserviceAssign.PaperDispatchDate = PaperDispatchDate;
            objserviceAssign.Amount = Amount;
            objserviceAssign.Discount = Discount;
            decimal AWithoutDis =/* quant **/ Amount;
            objserviceAssign.DiscountAmount =/*objserviceAssign.Quantity**/ AWithoutDis * (Discount / 100);
            objserviceAssign.GrandTotal = AWithoutDis - objserviceAssign.DiscountAmount;
            objserviceAssign.BillNo = BillNo;
            objserviceAssign.PaymentType = payment;
            //var date = DateTime.Now.ToShortDateString();
            //objserviceAssign.UpdatedDate = Convert.ToDateTime(date);
            objserviceAssign.UpdatedDate = Convert.ToDateTime(PaymentDate);
            if (objserviceAssign.status)
            {
                objserviceAssign.EndedDate = objserviceAssign.PaperDispatchDate.AddDays(Convert.ToInt32(Duration));
                objserviceAssign.status = false;
            }
            else
            {
                if (objserviceAssign.EndedDate.Date > DateTime.Now.Date)
                {
                    objserviceAssign.EndedDate = objserviceAssign.EndedDate.AddDays(Convert.ToInt32(Duration));
                }
                else if (objserviceAssign.EndedDate.Date < DateTime.Now.Date)
                {
                    objserviceAssign.EndedDate = objserviceAssign.PaperDispatchDate.AddDays(Convert.ToInt32(Duration));
                }
            }

            db.SaveChanges();

            Account objAccount = new Account();
            objAccount.Amount = objserviceAssign.GrandTotal;
            objAccount.Paymentdate = Convert.ToDateTime(PaymentDate);
            objAccount.Nepalidate = Nepalidate;
            objAccount.BankName = BankName;
            objAccount.BankAcc = ACNumber;
            objAccount.DiscountAmount = objserviceAssign.DiscountAmount;
            objAccount.CustomerId = objserviceAssign.CustomerId;
            objAccount.NewspaperId = objserviceAssign.NewspaperId;
            objAccount.CreatedBy = "counter";
            objAccount.CreatedDate = DateTime.Now;
            objAccount.UpdateDate = DateTime.Now;
            objAccount.UpdatedBy = "counter";
            objAccount.fiscalyear = db.FiscalYear.FirstOrDefault(m => m.Status == true);
            objAccount.FiscalYear = db.FiscalYear.FirstOrDefault(m => m.Status == true).NepYear;
            objAccount.BillNo = Convert.ToInt32(BillNo);
            objAccount.Payoption = PaymentType;
            db.Account.Add(objAccount);
            db.SaveChanges();
            log.AddActivity("Customer Successfully activated");
            string accId = db.Account.OrderByDescending(m => m.Id).FirstOrDefault().Id.ToString();


            // return ReportCounter(BillNo.ToString());
            return RedirectToAction("ReportCounter", "Counter", new { BillNo = accId });//


        }

    

        public ActionResult ReportCounter(string BillNo)
        {
            int BillNum = Convert.ToInt32(BillNo);
            var acc = db.Account.Find(BillNum);
            var cus = (from s in db.ServiceAssign
                       from c in db.Customer
                       from n in db.Service
                       where s.NewspaperId == n.Id && s.CustomerId == c.Id && s.NewspaperId == acc.NewspaperId && s.CustomerId==acc.CustomerId 
                       select new
                       {
                           Customer = c,
                           ServiceAssign = s,
                           NewsPaper = n
                       }).ToList().FirstOrDefault();
            int billno = Convert.ToInt32(BillNum);
            
            Account objaccount = db.Account.FirstOrDefault(m => m.Id == billno);


            PrintVM objPrint = new PrintVM();
            objPrint.Address = cus.Customer.Address;
            objPrint.Amount = cus.ServiceAssign.Amount;
            objPrint.AmountWord = NumToWords(/*cus.ServiceAssign.Quantity**/(cus.ServiceAssign.Amount) - cus.ServiceAssign.DiscountAmount)+ " मात्र";
            objPrint.BankAcc = objaccount.BankAcc;
            objPrint.BankName = objaccount.BankName;
            objPrint.CustomerId = objaccount.CustomerId.ToString();
            objPrint.CustomerName = cus.Customer.FirstName;
            objPrint.DiscountAmount = cus.ServiceAssign.DiscountAmount;
            objPrint.fiscalyear =acc.fiscalyear.NepYear;
            objPrint.EndedDate = cus.ServiceAssign.EndedDate.ToShortDateString();
            objPrint.GrandTotal = objaccount.Amount;
            objPrint.NepaliDate = cus.ServiceAssign.NepaliDate;
            objPrint.NewspaperName = cus.NewsPaper.NewsPaperName;
            objPrint.Phone = cus.Customer.MPhone;
            objPrint.Quantity = cus.ServiceAssign.Quantity;
            objPrint.Remarks = "";
            objPrint.StartDate = cus.ServiceAssign.PaperDispatchDate.ToShortDateString();
            objPrint.Subject = "";
            objPrint.BillNo =objaccount.BillNo.ToString();

            
            return View(objPrint);
        }


        public ActionResult BillView()
        {
            if (db.Account.Any())
            {
                // BillVM objBill = new BillVM();


                var cus = (from a in db.Account
                           from c in db.Customer
                           from n in db.Service
                           where a.NewspaperId == n.Id && a.CustomerId == c.Id
                           select new
                           {
                               Customer = c,
                               Account = a,
                               NewsPaper = n
                           }).ToList(); // as IEnumerable<CounterVM>;



                List<BillVM> lstBill = new List<BillVM>();


                foreach (var item in cus)
                {
                    BillVM Bill = new BillVM();
                    Bill.BillNo = item.Account.BillNo.ToString();
                    Bill.CustomerId = item.Customer.CustomerId;
                    Bill.fiscalyear = item.Account.FiscalYear;
                    Bill.CustomerName = item.Customer.FirstName;
                    Bill.PrintedDate = item.Account.Nepalidate;
                    Bill.NewspaperName = item.NewsPaper.NewsPaperName;
                    Bill.Address = item.Customer.Address;
                    Bill.AccountId = item.Account.Id;
                    lstBill.Add(Bill);


                }

                lstBill[0].BillNos = new SelectList(db.Account, "BillNo", "BillNo");
                lstBill[0].Newspapers = new SelectList(db.Newspaper, "NewspaperName", "NewspaperName");




                return View(lstBill);
            }
            else
            {
                return RedirectToAction("Index");
            }


        }
        







        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}




           
        [HttpPost]
        public JsonResult CheckDate(int id)
        {
            try
            {
                ServiceAssign serviceAssigned = db.ServiceAssign.Find(id);
                
                if ((serviceAssigned.BillNo!=null))
                {
                    if (serviceAssigned.EndedDate > DateTime.Now)
                    {
                        return Json("true");
                    }
                }

                return Json("false");
            }
            catch
            {
                return Json("false");
            }
        }


        private string[] nepaliNum = { "सुन्य", "एक", "दुई", "तीन", "चार", "पाँच", "छ", "सात", "आठ", "नौ", "दस", "एघार", "बाह्र", "तेह्र", "चौध", "पन्ध्र", "सोह्र", "सत्र", "अठाह्र", "उन्नाइस", "बीस", "एकाइस", "बाइस", "तेइस", "चौबीस", "पचीस", "छब्बीस", "सत्ताइस", "अठ्ठाइस", "उनन्तीस", "तीस", "एकतीस", "बतीस", "तेतीस", "चौतीस", "पैतीस", "छतीस", "सरतीस", "अरतीस", "उननचालीस", "चालीस", "एकचालीस", "बयालिस", "तीरचालीस", "चौवालिस", "पैंतालिस", "छयालिस", "सरचालीस", "अरचालीस", "उननचास", "पचास", "एकाउन्न", "बाउन्न", "त्रिपन्न", "चौवन्न", "पच्पन्न", "छपन्न", "सन्ताउन्न", "अन्ठाउँन्न", "उनान्न्साठी ", "साठी", "एकसाठी", "बासाठी", "तीरसाठी", "चौंसाठी", "पैसाठी", "छैसठी", "सत्सठ्ठी", "अर्सठ्ठी", "उनन्सत्तरी", "सतरी", "एकहत्तर", "बहत्तर", "त्रिहत्तर", "चौहत्तर", "पचहत्तर", "छहत्तर", "सत्हत्तर", "अठ्हत्तर", "उनास्सी", "अस्सी", "एकासी", "बयासी", "त्रीयासी", "चौरासी", "पचासी", "छयासी", "सतासी", "अठासी", "उनान्नब्बे", "नब्बे", "एकान्नब्बे", "बयान्नब्बे", "त्रियान्नब्बे", "चौरान्नब्बे", "पंचान्नब्बे", "छयान्नब्बे", "सन्तान्‍नब्बे", "अन्ठान्नब्बे", "उनान्सय" };
        public string NumToWords(decimal nums)
        {
            var num = Convert.ToString(nums);
            string num1 = num.Split('.')[0];
            string num2 = num.Split('.')[1].Substring(0, 2);
            string str = "";
            int charCount = num1.Length;
            if (num1.Length > 10 || num1.Length == 0)
            {
                str = "Error Converting Code";
                return str;
            }

            if (num1.Length != 10)
            {
                for (int i = 0; i <= 10 - charCount - 1; i++)
                {
                    num1 = "0" + num1;
                }
            }

            int[] place = new int[6];
            place[0] = Convert.ToInt32(num1.Substring(1, 1));
            place[1] = Convert.ToInt32(num1.Substring(1, 2));
            place[2] = Convert.ToInt32(num1.Substring(3, 2));
            place[3] = Convert.ToInt32(num1.Substring(5, 2));
            place[4] = Convert.ToInt32(num1.Substring(7, 1));
            place[5] = Convert.ToInt32(num1.Substring(8, 2));
            for (int i = 0; i <= 6 - 1; i++)
            {
                if (place[i] != 0)
                {
                    switch (i)
                    {
                        case (0):
                            {
                                str += nepaliNum[place[i]] + " " + "अरब ";
                                break;
                            }
                        case (1):
                            {
                                str += nepaliNum[place[i]] + " " + "करोड ";
                                break;
                            }
                        case (2):
                            {
                                str += nepaliNum[place[i]] + " " + "लाख ";
                                break;
                            }
                        case (3):
                            {
                                str += nepaliNum[place[i]] + " " + "हजार ";
                                break;
                            }
                        case (4):
                            {
                                str += nepaliNum[place[i]] + " " + "सय ";
                                break;
                            }
                        case (5):
                            {
                                str += nepaliNum[place[i]] + " ";
                                break;
                            }
                        default:
                            {
                                str += " सुन्य ";
                                break;
                            }
                    }
                }
            }
            if (Convert.ToInt32(num2) != 0)
            {
                str += "रुपैयाँ र" + " " + nepaliNum[Convert.ToInt32(num2)] + " पैसा ";
            }
            else
            {
                str += "रुपैयाँ ";
            }

            return str;
        }




    }
}
