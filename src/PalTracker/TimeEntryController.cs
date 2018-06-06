using System;
using Microsoft.AspNetCore.Mvc;

namespace PalTracker
{
    [Route("/time-entries")]
    public class TimeEntryController : Controller
    {
        private readonly ITimeEntryRepository _repository;  
        private readonly IOperationCounter<TimeEntry> _counter;

        public TimeEntryController(ITimeEntryRepository repository, IOperationCounter<TimeEntry> counter)
        {
            _repository = repository;
            _counter = counter;
        }

        [HttpGet("{id}", Name = "GetTimeEntry")]
        public IActionResult Read(long id)
        {
            _counter.Increment(TrackedOperation.Read);
            return _repository.Contains(id) ? (IActionResult) Ok(_repository.Find(id)) : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] TimeEntry timeEntry)
        {
            _counter.Increment(TrackedOperation.Create);
            var newTimeEntry = _repository.Create(timeEntry);
            return CreatedAtRoute("GetTimeEntry", new {id = newTimeEntry.Id}, newTimeEntry);
        }

        [HttpGet]
        public IActionResult List()
        {
           _counter.Increment(TrackedOperation.List);
           return Ok(_repository.List());
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TimeEntry timeEntry)
        {
            _counter.Increment(TrackedOperation.Update);
            return _repository.Contains(id) ? (IActionResult) Ok(_repository.Update(id, timeEntry)) : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _counter.Increment(TrackedOperation.Delete);

            if (!_repository.Contains(id))
            {
                return NotFound();
            }

            _repository.Delete(id);

            return NoContent();
        }
            
    }
}