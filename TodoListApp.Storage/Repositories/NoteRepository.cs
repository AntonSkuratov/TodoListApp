using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.Core.Entities;
using TodoListApp.Core.Infrastructure.Security;
using TodoListApp.Core.Interfaces;
using TodoListApp.Core.Records.Requests;
using TodoListApp.Core.Records.Response;

namespace TodoListApp.Storage.Repositories
{
	public class NoteRepository : INoteRepository
	{		
		public void CreateNote(CreateNoteRequest noteRequest)
		{
			using (var context = new TodoListContext())
			{
				var user = context.Users.First(u => u.Id == noteRequest.UserId);
				var note = new Note
				{
					Title = noteRequest.Title,
					Description = noteRequest.Description,
					User = user
				};
				context.Notes.Add(note);
				context.SaveChanges();
			}		
		}

		public void DeleteNote(int id)
		{
			using (var context = new TodoListContext())
			{
				var note = context.Notes.First(x => x.Id == id);
				context.Notes.Remove(note);
				context.SaveChanges();
			}		
		}

		public List<GetAllNotesResponse> GetAllNotes()
		{
			using (var context = new TodoListContext())
			{
				var notes = context.Notes
				.Include(n => n.User)
				.ToList();
				var allNotes = new List<GetAllNotesResponse>();
				foreach (var note in notes)
				{
					var noteResponse = new GetAllNotesResponse
					(
						note.Id,
						note.Title!,
						note.Description!,
						note.UserId
					);
					allNotes.Add(noteResponse);
				}
				return allNotes.ToList();
			}			
		}

		public List<GetAllNotesByUserIdResponse> GetAllNotes(int userId)
		{
			using (var context = new TodoListContext())
			{
				var notes = context.Users
				.Include(u => u.Notes)
				.First(u => u.Id == userId)
				.Notes
				.ToList();
				var notesResponse = new List<GetAllNotesByUserIdResponse>();
				foreach (var note in notes)
				{
					var noteResponse = new GetAllNotesByUserIdResponse
					(
						note.Id,
						note.Title!,
						note.Description!
					);
					notesResponse.Add(noteResponse);
				}
				return notesResponse.ToList();
			}	
		}
	}
}
