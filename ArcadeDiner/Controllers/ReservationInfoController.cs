using ArcadeDiner.Models;
using ArcadeDiner.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace ArcadeDiner.Controllers
{
	[Route("api/ReservationInfo")]
	[ApiController]
	public class ReservationInfoController : ControllerBase
	{
		[HttpGet]
		public IEnumerable<ReservationInfo> ReservationList()
		{
			var reservationInfo = new List<ReservationInfo>();
			using (var context = new ArcadeDinerContext())
			{
				reservationInfo = context.ReservationInfos.OrderByDescending(x => x.SubmissionDateTime).ToList();
			}

			return reservationInfo;
		}

		[HttpGet]
		[Route("update")]
		public async IAsyncEnumerable<ReservationInfo> ReservationListById(int reservationId)
		{

			using (var context = new ArcadeDinerContext())
			{
				var reservation = await context.ReservationInfos.Where(x => x.ReservationId == reservationId).FirstOrDefaultAsync();
				yield return reservation ?? new ReservationInfo();
			}
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteReservationAsync(int reservationId)
		{
			try
			{
				using (var context = new ArcadeDinerContext())
				{
					var reservationToDelete = await context.ReservationInfos.Where(x => x.ReservationId == reservationId).FirstOrDefaultAsync();
					if (reservationToDelete != null && reservationId > 0)
					{
						context.ReservationInfos.Remove(reservationToDelete);
						await context.SaveChangesAsync();
						return Ok(200);
					}
					Console.WriteLine($"{nameof(reservationToDelete)} was null or {nameof(reservationId)} was missing in {nameof(ReservationInfoController)}.{nameof(DeleteReservationAsync)}");
					return BadRequest(reservationId);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception thrown in {nameof(ReservationInfoController)}.{nameof(DeleteReservationAsync)} Message:{ex.Message}");
				return BadRequest(reservationId);
			}
		}

		[HttpPost]
		public async Task<IActionResult> InsertReservationAsync([FromBody] object resInfo)
		{
			
			try
			{
				using (var context = new ArcadeDinerContext())
				{
					//TODO: fix parameter resInfo, unboxing for now to complete the request
					//TODO: get max resvervation number and increment instead of user entry/random numbers and no duplicate reservation numbers

					var reservation = UnboxReservationObject(resInfo);
					if (string.IsNullOrEmpty(reservation.PartyName) || reservation.ReservationId == 0)
					{
						return BadRequest(reservation);
					}

					reservation.LastUpdateDate = DateTime.Now;
					reservation.SubmissionDateTime = DateTime.Now;
					reservation.ReservationNumber = reservation.ReservationNumber > 0 ? reservation.ReservationNumber : new Random().Next(1000, 9999999); 
					context.ReservationInfos.Add(reservation);
					await context.SaveChangesAsync();
					return Ok(resInfo);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception thrown in {nameof(ReservationInfoController)}.{nameof(InsertReservationAsync)} Message:{ex.Message}");
				return BadRequest(resInfo);
			}
		}

		[HttpPut]
		public async Task<IActionResult> UpdateReservationAsync([FromBody]object resInfo)
		{
			try
			{
				var resInfoObj = UnboxReservationObject(resInfo);
				using (var context = new ArcadeDinerContext())
				{
					var reservation = await context.ReservationInfos.Where(x => x.ReservationId == resInfoObj.ReservationId).FirstOrDefaultAsync();
					if(reservation?.ReservationId > 0)
					{
						reservation.PartyName = resInfoObj.PartyName;
						reservation.ReservationNumber = resInfoObj.ReservationNumber;
						reservation.ReservationDate = resInfoObj.ReservationDate;
						reservation.LastUpdateDate = DateTime.Now;
						reservation.ReservationTime = resInfoObj.ReservationTime;
						reservation.NumberInParty = resInfoObj.NumberInParty;
						reservation.IsFulfilled = resInfoObj.IsFulfilled;

						context.Entry(reservation).State = EntityState.Modified;
						context.SaveChanges();
						return Ok(resInfoObj);
					}

					return BadRequest(resInfoObj);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception thrown in {nameof(ReservationInfoController)}.{nameof(UpdateReservationAsync)} Message:{ex.Message}");
				return BadRequest(resInfo);
			}
		}

		private ReservationInfo UnboxReservationObject(object resInfo)
		{
			if(resInfo != null)
			{

				JObject obj = JObject.Parse(resInfo.ToString());
				if (obj.HasValues)
				{
					return new ReservationInfo
					{
						PartyName = obj.Value<string>("partyName") ?? "Not Found",
						NumberInParty = obj.Value<int>("numberInParty"),
						ReservationDate = obj.Value<DateTime>("reservationDate"),
						ReservationTime = TimeSpan.Parse(obj.Value<string>("reservationTime")),
						ReservationNumber = obj.Value<int>("reservationNumber"),
						Email = obj.Value<string>("email"),
						IsFulfilled = obj.Value<bool>("isFulfilled"),
						SubmissionDateTime = obj.Value<DateTime>("submissionDate"),
						ReservationId = obj.Value<int>("reservationId")
					};
				}
				return new ReservationInfo();
			}

			Console.WriteLine($"{nameof(resInfo)}.{typeof(object)} was null in {nameof(ReservationInfoController)}.{nameof(UnboxReservationObject)}");
			return new ReservationInfo();
		}
	}
}