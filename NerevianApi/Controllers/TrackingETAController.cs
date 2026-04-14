using Microsoft.AspNetCore.Mvc;
using NerevianApi.Models; 

namespace NerevianApi.Controllers
    {
        [Route("api/[controller]")] 
        [ApiController]
        public class TrackingETAController : ControllerBase
        {
            [HttpGet("{id}")]
            public ActionResult<TrackingETAResponse> GetTrackingInfo(string id)
            {
                if (id == "EXP-8822")
                {
                    return Ok(new TrackingETAResponse
                    {
                        ReferenceCode = "EXP-8822",
                        Route = "NGB -> ITA",
                        EtaDate = "16 Oct 2013",
                        GlobalStatus = "En Aduanas",
                        ContainerNumber = "CMAU7766552",

                        History = new List<TrackingStatusItem>
                        {
                            new TrackingStatusItem { Title = "almacén", Time = "26 Sep 2023, 09:00", IsCompleted = true },
                            new TrackingStatusItem { Title = "Despacho", Time = "28 Sep 2023, 16:45", IsCompleted = true },
                            new TrackingStatusItem { Title = "Salida", Time = "01 Oct 2023, 10:15", IsCompleted = true },
                            new TrackingStatusItem { Title = "En tránsito marítimo", Time = "05 Oct 2023, 08:00", IsCompleted = false },
                            new TrackingStatusItem { Title = "Llegada a puerto", Time = "12 Oct 2023, 14:30", IsCompleted = false }
                        }

                    });
                }
                else if (id == "EXP-8823")
                {
                    return Ok(new TrackingETAResponse
                    {
                        ReferenceCode = "EXP-8823",
                        Route = "QIN -> MAD",
                        EtaDate = "20 Oct 2028",
                        GlobalStatus = "Pendiente", 
                        ContainerNumber = "MEDU3344553"
                    });
                }
                else
                {
                    return Ok(new TrackingETAResponse
                    {
                        ReferenceCode = "EXP-8821",
                        Route = "SHA -> VLC",
                        EtaDate = "13 Oct 2032",
                        GlobalStatus = "En Tránsito",
                        ContainerNumber = "MSKU9988771"
                    });
                }
            }
        }
    }