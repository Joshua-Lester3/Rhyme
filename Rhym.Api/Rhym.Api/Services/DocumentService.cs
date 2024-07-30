using Microsoft.EntityFrameworkCore;
using Rhym.Api.Data;
using Rhym.Api.Models;
using Rhym.Api.Requests;
using Rhym.Api.Dtos;

namespace Rhym.Api.Services;

public class DocumentService
{
	private readonly AppDbContext _context;
	private static object _changingDocumentLock = new();
	private static object _addingDocumentLock = new();
	private static object _deletingDocumentLock = new();
	private static object _togglingSharedLock = new();
	public DocumentService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<List<DocumentDto>> GetDocumentListAsync(string userId)
	{
		return await _context.Documents.Where(document => document.UserId == userId).Select(document => new DocumentDto
		{
			UserId = document.UserId,
			DocumentId = document.DocumentId,
			Title = document.Title,
			Content = document.Content.Length > 150 ? document.Content.Substring(0, 150) : document.Content,
			IsShared = document.IsShared,
			LastSaved = document.LastSaved,
		}).ToListAsync();
	}

	public async Task<Document> PostDocumentAsync(DocumentDto request)
	{
		Document? foundDocument = null;
		if (request.DocumentId != -1)
		{
			foundDocument = await _context.Documents.
				Where(dbDocument => dbDocument.DocumentId == request.DocumentId).
				FirstOrDefaultAsync();
		}
		if (foundDocument is null)
		{
			lock (_addingDocumentLock)
			{
				if (request.DocumentId != -1)
				{
					foundDocument = _context.Documents.
						FirstOrDefault(dbDocument => dbDocument.DocumentId == request.DocumentId);
				}

				if (foundDocument is null)
				{
					Document addedDocument = new Document
					{
						UserId = request.UserId,
						Content = request.Content,
						Title = request.Title,
						IsShared = request.IsShared,
						LastSaved = request.LastSaved
					};
					_context.Documents.Add(addedDocument);
					_context.SaveChanges();
					return addedDocument;
				}
				else
				{
					foundDocument.Content = request.Content;
					foundDocument.Title = request.Title;
					foundDocument.IsShared = request.IsShared;
					foundDocument.LastSaved = request.LastSaved;
					_context.SaveChanges();
					return foundDocument;
				}
			}
		}
		else
		{
			lock (_changingDocumentLock)
			{
				foundDocument.Content = request.Content;
				foundDocument.Title = request.Title;
				foundDocument.IsShared = request.IsShared;
				foundDocument.LastSaved = request.LastSaved;
				_context.SaveChanges();
				return foundDocument;
			}
		}
	}

	public async Task<DocumentDto?> GetDocumentDataAsync(string userId, int documentId)
	{
		var document = await _context.Documents
			.Where(document => document.DocumentId == documentId)
			.FirstOrDefaultAsync();
		if (document != null && (userId == document.UserId || document.IsShared))
		{
			return new DocumentDto
			{
				UserId = document.UserId,
				DocumentId = document.DocumentId,
				Title = document.Title,
				Content = document.Content,
				IsShared = document.IsShared,
			};
		}
		return null;
	}

	public async Task<bool> DeleteDocumentAsync(int documentId)
	{
		var foundDocument = await _context.Documents.FirstOrDefaultAsync(document => document.DocumentId == documentId);
		if (foundDocument is not null)
		{
			lock (_deletingDocumentLock)
			{
				foundDocument = _context.Documents.FirstOrDefault(document => document.DocumentId == documentId);
				if (foundDocument is not null)
				{
					_context.Documents.Remove(foundDocument);
					_context.SaveChanges();
					return true;
				}
				return false;
			}
		}
		return false;
	}

	public async Task<Document?> ToggleSharedAsync(int documentId, bool isShared)
	{
		var foundDocument = await _context.Documents.FirstOrDefaultAsync(document => document.DocumentId == documentId);
		if (foundDocument is not null)
		{
			lock (_togglingSharedLock)
			{
				foundDocument = _context.Documents.FirstOrDefault(document => document.DocumentId == documentId);
				if (foundDocument is not null)
				{
					foundDocument.IsShared = isShared;
					_context.SaveChanges();
				}
				return foundDocument;
			}
		}
		return null;
	}
}
