using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSRepository;
using RSData.Models;
using System.Text;
using static RSData.Models.Role;
using RSService.DTO;
using RSService.Validation;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using MimeKit;
using MailKit.Net.Smtp;

namespace RSService.BusinessLogic
{
    public class HangFireJobService
    {
        private readonly ISettingsRepository settingsRepository;
        private readonly IEventRepository eventRepository;
        private readonly RoomPlannerDevContext context;
        private readonly IUserRepository userRepository;
        private readonly IRoomRepository roomRepository;

        public HangFireJobService(RoomPlannerDevContext context)
        {
            this.context = context;
            userRepository = new UserRepository(context);
            eventRepository = new EventRepository(context);
            settingsRepository = new SettingsRepository(context);
            roomRepository = new RoomRepository(context);
        }


        public void EventReminder()
        { 

            //this will olways will have just one value, so it doesn't matter it's for in for;
            var emailremindervalue = settingsRepository.GetValueOfEmailReminderSettings();
            foreach (Settings set in emailremindervalue)
            {
                var events = eventRepository.GetEventsByDateTimeNow(Int32.Parse(set.Value));
                foreach (Event evnt in events)
                {
                    evnt.EventStatus = (int)EventStatusEnum.waitingReminder;
                    context.SaveChanges();
                    var usr = userRepository.GetUserById(evnt.AttendeeId);
                    var room = roomRepository.GetRoomById(evnt.RoomId);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("RoomSchedulerStefanini", "roomchedulerStefanini@gmail.com"));
                    message.To.Add(new MailboxAddress("User", usr.Email));
                    message.Subject = "Remainder";
                    message.Body = new TextPart("html")
                    {
                        Text = " You have a massage programmed for today! <br>"
                        + " DateStart: " + evnt.StartDate.TimeOfDay + "<br>"
                        + " Room Name: " + room.Name + "<br>"
                        + " Room Location: " + room.Location + "<br>"



                    };
                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);
                        client.Authenticate("roomchedulerStefanini@gmail.com", "admin123456");

                        client.Send(message);

                        client.Disconnect(true);
                    }

                }
            }
        }


        public void ChangeEventStatus()
        {
            var events = eventRepository.GetEventsByCurrentDate();

            foreach( Event evnt in events)
            {
                evnt.EventStatus = (int)EventStatusEnum.present;
                context.SaveChanges();

            }
        }


    }
}
