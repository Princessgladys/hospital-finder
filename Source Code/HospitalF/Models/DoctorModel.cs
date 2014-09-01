using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using HospitalF.Entities;

namespace HospitalF.Models
{
    public class DoctorModel
    {
        public int DoctorID { get; set; }
        public int HospitalID { get; set; }
        public string HospitalName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Fullname { get; set; }
        public int Gender { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public int PhotoID { get; set; }
        public string PhotoFilePath { get; set; }
        public int UploadedPerson { get; set; }
        public int SpecialityID { get; set; }
        public string SpecialityName { get; set; }
        public List<int> SpecialityList { get; set; }
        public List<int> WorkingDay { get; set; }

        public static List<DoctorEntity> LoadDoctorList(int hospitalId, string doctorName)
        {
            List<DoctorEntity> doctorList = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                doctorList = (from d in data.Doctors
                              from dh in d.Doctor_Hospitals
                              where (d.Last_Name + " " + d.First_Name).Contains(doctorName) &&
                                    dh.Hospital_ID == hospitalId &&
                                    d.Is_Active == true
                              select new DoctorEntity()
                              {
                                  Doctor_ID = d.Doctor_ID,
                                  First_Name = d.First_Name,
                                  Last_Name = d.Last_Name,
                                  Gender = d.Gender,
                                  Degree = d.Degree,
                                  Experience = d.Experience,
                                  Working_Day = d.Working_Day,
                                  Is_Active = d.Is_Active
                              }).ToList<DoctorEntity>();

                List<Speciality> specialities = null;
                Photo photo = new Photo();
                foreach (DoctorEntity doctor in doctorList)
                {
                    specialities = (from d in data.Doctors
                                    from ds in d.Doctor_Specialities
                                    where doctor.Doctor_ID == d.Doctor_ID
                                    select ds.Speciality).ToList<Speciality>();
                    photo = (from p in data.Photos
                             where doctor.Doctor_ID == p.Doctor_ID
                             select p).SingleOrDefault();

                    doctor.Specialities = specialities;
                    doctor.Photo = photo;
                }
            }
            return doctorList;
        }

        public static bool InsertDoctor(DoctorModel model)
        {
            using (TransactionScope ts = new TransactionScope())
            {

                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    string workingDay = "";
                    foreach (int wd in model.WorkingDay)
                    {
                        workingDay = workingDay + wd + ',';
                    }
                    if (workingDay.Length > 0)
                    {
                        workingDay = workingDay.Substring(0, workingDay.Length - 1);
                    }
                    Doctor doctor = new Doctor()
                                    {
                                        First_Name = model.FirstName,
                                        Last_Name = model.LastName,
                                        Gender = model.Gender == 1 ? true : false,
                                        Degree = model.Degree,
                                        Experience = model.Experience,
                                        Working_Day = string.IsNullOrEmpty(workingDay) ? null : workingDay,
                                        Is_Active = true
                                    };
                    data.Doctors.InsertOnSubmit(doctor);
                    data.SubmitChanges();

                    Photo photo = new Photo()
                    {
                        File_Path = model.PhotoFilePath,
                        Caption = model.LastName + " " + model.FirstName,
                        Add_Date = DateTime.Now,
                        Doctor_ID = doctor.Doctor_ID,
                        Uploaded_Person = model.UploadedPerson,
                        Is_Active = true
                    };

                    data.Photos.InsertOnSubmit(photo);
                    data.SubmitChanges();

                    foreach (int specilityId in model.SpecialityList)
                    {
                        Doctor_Speciality ds = new Doctor_Speciality()
                                               {
                                                   Doctor_ID = doctor.Doctor_ID,
                                                   Speciality_ID = specilityId,
                                                   Is_Active = true
                                               };
                        data.Doctor_Specialities.InsertOnSubmit(ds);
                        data.SubmitChanges();
                    }
                    Doctor_Hospital dh = new Doctor_Hospital()
                                         {
                                             Doctor_ID = doctor.Doctor_ID,
                                             Hospital_ID = model.HospitalID,
                                             Is_Active = true
                                         };
                    data.Doctor_Hospitals.InsertOnSubmit(dh);
                    data.SubmitChanges();
                    ts.Complete();
                }
            }
            return true;
        }

        public static DoctorEntity LoadDoctorById(int doctorId, int hospitalId)
        {
            DoctorEntity doctor = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                doctor = (from d in data.Doctors
                          from dh in d.Doctor_Hospitals
                          where d.Doctor_ID == doctorId &&
                                dh.Hospital_ID == hospitalId
                          select new DoctorEntity()
                          {
                              Doctor_ID = d.Doctor_ID,
                              First_Name = d.First_Name,
                              Last_Name = d.Last_Name,
                              Gender = d.Gender,
                              Degree = d.Degree,
                              Experience = d.Experience,
                              Working_Day = d.Working_Day
                          }).SingleOrDefault();
                if (doctor != null)
                {
                    List<Speciality> specialities = null;
                    Photo photo = new Photo();

                    specialities = (from d in data.Doctors
                                    from ds in d.Doctor_Specialities
                                    where doctor.Doctor_ID == d.Doctor_ID
                                    select ds.Speciality).ToList<Speciality>();
                    photo = (from p in data.Photos
                             where doctor.Doctor_ID == p.Doctor_ID
                             select p).SingleOrDefault();

                    doctor.Specialities = specialities;
                    doctor.Photo = photo;
                }
            }
            return doctor;
        }

        public static bool UpdateDoctor(DoctorModel model)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    string workingDay = "";
                    foreach (int wd in model.WorkingDay)
                    {
                        workingDay = workingDay + wd + ',';
                    }
                    if (workingDay.Length > 0)
                    {
                        workingDay = workingDay.Substring(0, workingDay.Length - 1);
                    }
                    Doctor doctor = (from d in data.Doctors
                                     where d.Doctor_ID == model.DoctorID
                                     select d).SingleOrDefault();
                    if (doctor != null)
                    {
                        doctor.First_Name = model.FirstName;
                        doctor.Last_Name = model.LastName;
                        doctor.Gender = model.Gender == 1 ? true : false;
                        doctor.Degree = model.Degree;
                        doctor.Experience = model.Experience;
                        doctor.Working_Day = string.IsNullOrEmpty(workingDay) ? null : workingDay;
                        doctor.Is_Active = true;
                        data.SubmitChanges();


                        var dsList = (from ds in data.Doctor_Specialities
                                      where ds.Doctor_ID == doctor.Doctor_ID
                                      select ds);
                        data.Doctor_Specialities.DeleteAllOnSubmit(dsList);
                        data.SubmitChanges();

                        foreach (int specilityId in model.SpecialityList)
                        {
                            Doctor_Speciality ds = new Doctor_Speciality()
                            {
                                Doctor_ID = doctor.Doctor_ID,
                                Speciality_ID = specilityId,
                                Is_Active = true
                            };
                            data.Doctor_Specialities.InsertOnSubmit(ds);
                            data.SubmitChanges();
                        }

                        Photo photo = (from p in data.Photos
                                       where p.Doctor_ID == doctor.Doctor_ID
                                       select p).SingleOrDefault();
                        if (photo != null)
                        {
                            photo.File_Path = model.PhotoFilePath;
                            photo.Uploaded_Person = model.UploadedPerson;
                            data.SubmitChanges();
                        }
                        else
                        {
                            Photo newPhoto = new Photo()
                            {
                                File_Path = model.PhotoFilePath,
                                Caption = model.LastName + " " + model.FirstName,
                                Add_Date = DateTime.Now,
                                Doctor_ID = doctor.Doctor_ID,
                                Uploaded_Person = model.UploadedPerson,
                                Is_Active = true
                            };

                            data.Photos.InsertOnSubmit(newPhoto);
                            data.SubmitChanges();
                        }
                    }

                    ts.Complete();
                }
            }
            return true;
        }

        public static bool DeactivateDoctor(int doctorId, int hospitalId)
        {
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                Doctor doctor = (from d in data.Doctors
                                 from dh in d.Doctor_Hospitals
                                 where d.Doctor_ID == doctorId &&
                                       dh.Hospital_ID == hospitalId
                                 select d).SingleOrDefault();
                if (doctor != null)
                {
                    doctor.Is_Active = false;
                    data.SubmitChanges();
                }
                return true;
            }
        }
    }
}