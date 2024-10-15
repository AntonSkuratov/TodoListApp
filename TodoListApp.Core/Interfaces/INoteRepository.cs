using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.Core.Entities;
using TodoListApp.Core.Records.Requests;
using TodoListApp.Core.Records.Response;

namespace TodoListApp.Core.Interfaces
{
    public interface INoteRepository
	{
		void CreateNote(CreateNoteRequest userRequest);		
		void DeleteNote(int id);
		List<GetAllNotesResponse> GetAllNotes();
		List<GetAllNotesByUserIdResponse> GetAllNotes(int userId);
	}
}
