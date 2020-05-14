namespace Wrio.Util

module MyUtil =
    let rec span (predicate : 'T -> bool) (lst : 'T list) = 
        if lst.IsEmpty then 
            ([], [])
        elif (predicate lst.Head) then
            let lst2, lst3 = span predicate lst.Tail
            (lst.Head :: lst2, lst3)
        else
            ([], lst)
    
    (* -- haskell definision
     span _ []         = ([], [])
     span p xs@(x:xs')
         | p x         = (x:ys, zs)
         | otherwise   = ([], xs)
         where
         (ys, zs) = span p xs'
    *)