---
GDPOP Algorithm - a variant of DPOCL that enables the construction of plans with more hierarchical structure.
---

1. **Step 1**: First, primitive tasks are completely ground, then (non-primitive) composite tasks into ground decomposed hierarchical task networks with height cutoff ("h_max"). 

1. **Step 2**: GDPOP is a propositional partial order planner; GDPOP creates new plans for all possible task insertions when at "Add task to Repair Open Condition". Tasks can either be primitive, or be composite with height "h" up to "h_max". GDPOP tests different heuristics that include hierarchical depth in order to dynamically incentivize inserting composite tasks, thereby promoting deeper hierarchical structure while still finding solution plans efficiently.

* E3 Selection with Add Reuse Heuristic (VHPOP) performed best on test problems ("travel test")

---
David R. Winer
---

