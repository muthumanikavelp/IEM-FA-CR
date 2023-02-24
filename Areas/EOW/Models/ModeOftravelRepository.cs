using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.EOW.Models
{
    public interface ModeOftravelRepository
    {
        IEnumerable<ModeOfTravelEntity> SelectTravelMode();
        IEnumerable<ModeOfTravelEntity> SelectTravelMode(string modename);
        string SubmitMode(ModeOfTravelEntity mode);
        IEnumerable<ModeOfTravelEntity> EditMode(int id);
        string SubmitEditMode(ModeOfTravelEntity mode);
        string DeleteMode(int id);
        string DeleteTravel(int id);
    }
    public interface TravelClassRepository
    {
        IEnumerable<TravelClassEntity> SelectModeOfTravel();
        IEnumerable<TravelClassEntity> SelectTravelSearch(string Travelclass,string travelmode);
        IEnumerable<TravelClassEntity> SelectTravelSearch();
        string SubmitTravel(TravelClassEntity mode);
        IEnumerable<TravelClassEntity> EditTravel(int id);
        string SubmitEditTravel(TravelClassEntity mode);
        string DeleteTravel(int id);
       
    }
}