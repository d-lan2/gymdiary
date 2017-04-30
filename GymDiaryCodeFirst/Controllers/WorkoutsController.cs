﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GymDiaryCodeFirst.DAL;
using GymDiaryCodeFirst.Models;
using GymDiaryCodeFirst.Models.ViewModels;

namespace GymDiaryCodeFirst.Views
{
    public class WorkoutsController : Controller
    {
        private GymDiaryContext db = new GymDiaryContext();

        // GET: Workouts
        public ActionResult Index()
        {
            return View(db.Workouts.ToList());
        }

        // GET: Workouts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<ExerciseStats> workout = db.ExerciseStats.Where(w => w.WorkoutId == id).ToList();
            if (workout == null)
            {
                return HttpNotFound();
            }
            foreach(var item in workout)
            {
                item.Exercise = db.Exercises.Find(item.ExerciseId);
                item.Workout = db.Workouts.Find(item.WorkoutId);
            }
            return View(workout);
        }

        public Workout AddExercise(ExerciseStats e, Workout workout)
        {
            workout.Exercises.Add(e);
            return workout;
        }

        // GET: Workouts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Workouts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkoutId,UserId,Name,Date")] Workout workout)
        {
            if (ModelState.IsValid)
            {
                db.Workouts.Add(workout);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(workout);
        }

        // GET: Workouts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workout workout = db.Workouts.Find(id);
            if (workout == null)
            {
                return HttpNotFound();
            }
            AddExercisesToWorkoutFromDB(workout);

            WorkoutExerciseListViewModel model = new WorkoutExerciseListViewModel();

            model.Exercises = db.Exercises;
            model.Workout = workout;

            return View(model);
        }

        public Workout AddExercisesToWorkoutFromDB(Workout workout)
        {
            workout.Exercises = db.ExerciseStats.Where(e => e.WorkoutId == workout.WorkoutId).ToList();

            foreach (var item in workout.Exercises)
            {
                item.Exercise = db.Exercises.Find(item.ExerciseId);
                item.Workout = db.Workouts.Find(item.WorkoutId);
            }
            return workout;
        }

        // POST: Workouts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "WorkoutId,UserId,Name,Date")] Workout workout*/ WorkoutExerciseListViewModel viewModel)
        {
            var workout = viewModel.Workout;
            if (ModelState.IsValid)
            {
                var workoutInDb = db.Workouts.Single(x => x.WorkoutId == workout.WorkoutId);
                workoutInDb.Name = workout.Name;
                

                foreach (var e in workout.Exercises)
                {
                    var eInDb = db.ExerciseStats.Single(x => x.ExerciseStatsId == e.ExerciseStatsId);
                    eInDb.ExerciseId = e.ExerciseId;
                    eInDb.WeightInKg = e.WeightInKg;
                    eInDb.Sets = e.Sets;
                    eInDb.Reps = e.Reps;
                    eInDb.Minutes = e.Minutes;
                    db.SaveChanges();
                } 
                //What happens when an exercise has been removed/added?
                
                //Impliment adding and deleting an exercise

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Workouts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workout workout = db.Workouts.Find(id);
            if (workout == null)
            {
                return HttpNotFound();
            }
            return View(workout);
        }

        // POST: Workouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Workout workout = db.Workouts.Find(id);
            db.Workouts.Remove(workout);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
