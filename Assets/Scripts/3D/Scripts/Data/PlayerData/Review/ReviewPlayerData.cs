using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ReviewPlayerData<T1, T2> where T2 : ItemReviewPlayerData<T1>, new() {

    public List<T2> Reviews { get; set; }

    public ReviewPlayerData() {
        Reviews = new List<T2>();
    }

    protected virtual bool ItemsEqual(T1 t1, T1 t2) {
        return t1.Equals(t2);
    }

    public bool ContainsReview(T1 p) {
        var rev = (from r in Reviews where ItemsEqual(r.Item, p) select r).FirstOrDefault();
        return rev != null;
    }

    public void AddReview(T1 i) {
        if (!ContainsReview(i)) {
            var r = new T2();
            r.Item = i;
            Reviews.Add(r);
        }
    }

    public T2 GetReview(T1 p) {
        var rev = (from r in Reviews where ItemsEqual(r.Item, p) select r).FirstOrDefault();
        return rev;
    }

    public T2 GetOrCreateReview(T1 p) {
        var rev = (from r in Reviews where ItemsEqual(r.Item, p) select r).FirstOrDefault();
        if (rev == null) {
            rev = new T2();
            rev.Item = p;
            Reviews.Add(rev);
        }
        return rev;
    }

    public List<T2> GetCurrentReviews() {
        return (from r in Reviews where r.NeedsReview() select r).ToList();
    }

    public IEnumerable<T2> GetWeakReviews() {
        return from r in Reviews where r.Rank < 3 select r;
    }

    public IEnumerable<T2> GetModerateReviews() {
        return from r in Reviews where r.Rank >= 3 && r.Rank < 5 select r;
    }

    public IEnumerable<T2> GetStrongReviews() {
        return from r in Reviews where r.Rank >= 5 select r;
    }

    public IEnumerable<T2> GetReviewsForRank(int rank) {
        return from r in Reviews where r.Rank >= rank select r;
    }

}
