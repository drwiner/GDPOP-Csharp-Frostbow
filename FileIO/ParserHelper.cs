using BoltFreezer.Interfaces;
using BoltFreezer.PlanTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BoltFreezer.FileIO
{
    public static class ParserHelper
    {
        public static int CheckForConditionalEffect(Domain domain, List<IPredicate> effects, string [] words, int i)
        {
            // Check for a conditional effect.
            // THIS SHOULD PROBABLY BE CONDENSED
            if (words[i].Equals("(forall") || words[i].Equals("(when"))
            {
                // Create a new axiom object.
                Axiom axiom = new Axiom();

                if (words[i].Equals("(forall"))
                {
                    // Read in the axiom's terms.
                    while (!Regex.Replace(words[++i], @"\t|\n|\r", "").Equals("(when"))
                        axiom.Terms.Add(new Term(Regex.Replace(words[i], @"\t|\n|\r|[()]", "")));
                }

                // If the preconditions are conjunctive.
                if (Regex.Replace(words[++i], @"\t|\n|\r", "").Equals("(and"))
                {
                    // Initialize a parentheses stack counter.
                    int parenStack = 1;
                    i++;

                    // Use the stack to loop through the conjunction.
                    while (parenStack > 0)
                    {
                        // Check for an open paren.
                        if (words[i][0] == '(')
                        {
                            // Create new predicate.
                            Predicate pred = new Predicate();

                            // Check for a negative effect.
                            if (words[i].Equals("(not"))
                            {
                                // Iterate the counter.
                                i++;

                                // Set the effect's sign to false.
                                pred.Sign = false;
                            }

                            // Name the predicate.
                            pred.Name = Regex.Replace(words[i++], @"\t|\n|\r|[()]", "");

                            // Read in the terms.
                            while (words[i][words[i].Length - 1] != ')')
                                pred.Terms.Add(new Term(Regex.Replace(words[i++], @"\t|\n|\r|[()]", "")));

                            // Read the last term.
                            pred.Terms.Add(new Term(Regex.Replace(words[i++], @"\t|\n|\r|[()]", "")));

                            // Add the predicate to the axiom's preconditions.
                            axiom.Preconditions.Add(pred);
                        }

                        // Check for a close paren.
                        if (words[i][words[i].Length - 1] == ')')
                            parenStack--;
                    }
                }
                else
                {
                    // Check for an open paren.
                    if (words[i][0] == '(')
                    {
                        // Create new predicate.
                        Predicate pred = new Predicate();

                        // Check for a negative effect.
                        if (words[i].Equals("(not"))
                        {
                            // Iterate the counter.
                            i++;

                            // Set the effect's sign to false.
                            pred.Sign = false;
                        }

                        // Name the predicate.
                        pred.Name = Regex.Replace(words[i++], @"\t|\n|\r|[()]", "");

                        // Read in the terms.
                        while (words[i][words[i].Length - 1] != ')')
                            pred.Terms.Add(new Term(Regex.Replace(words[i++], @"\t|\n|\r|[()]", "")));

                        // Read the last term.
                        pred.Terms.Add(new Term(Regex.Replace(words[i], @"\t|\n|\r|[()]", "")));

                        // Add the predicate to the axiom's preconditions.
                        axiom.Preconditions.Add(pred);
                    }
                }

                // If the preconditions are conjunctive.
                if (Regex.Replace(words[++i], @"\t|\n|\r", "").Equals("(and"))
                {
                    // Initialize a parentheses stack counter.
                    int parenStack = 1;
                    i++;

                    // Use the stack to loop through the conjunction.
                    while (parenStack > 0)
                    {
                        // Check for an open paren.
                        if (words[i][0] == '(')
                        {
                            // Create new predicate.
                            Predicate pred = new Predicate();

                            parenStack++;

                            // Check for a negative effect.
                            if (words[i].Equals("(not"))
                            {
                                // Iterate the counter.
                                i++;

                                // Set the effect's sign to false.
                                pred.Sign = false;

                                parenStack++;
                            }

                            // Name the predicate.
                            pred.Name = Regex.Replace(words[i++], @"\t|\n|\r|[()]", "");

                            // Read in the terms.
                            while (words[i][words[i].Length - 1] != ')')
                                pred.Terms.Add(new Term(Regex.Replace(words[i++], @"\t|\n|\r|[()]", "")));

                            // Read the last term.
                            pred.Terms.Add(new Term(Regex.Replace(words[i++], @"\t|\n|\r|[()]", "")));

                            // Add the predicate to the axiom's effects.
                            axiom.Effects.Add(pred);
                        }

                        // Check for a close paren.
                        if (words[i - 1][words[i - 1].Length - 1] == ')')
                            parenStack--;

                        if (words[i - 1].Length > 1)
                            if (words[i - 1][words[i - 1].Length - 2] == ')')
                                parenStack--;

                        if (words[i - 1].Length > 2)
                            if (words[i - 1][words[i - 1].Length - 3] == ')')
                                parenStack--;
                    }
                }
                else
                {
                    // Check for an open paren.
                    if (words[i][0] == '(')
                    {
                        // Create new predicate.
                        Predicate pred = new Predicate();

                        // Check for a negative effect.
                        if (words[i].Equals("(not"))
                        {
                            // Iterate the counter.
                            i++;

                            // Set the effect's sign to false.
                            pred.Sign = false;
                        }

                        // Name the predicate.
                        pred.Name = Regex.Replace(words[i++], @"\t|\n|\r|[()]", "");

                        // Read in the terms.
                        while (words[i][words[i].Length - 1] != ')')
                            pred.Terms.Add(new Term(Regex.Replace(words[i++], @"\t|\n|\r|[()]", "")));

                        // Read the last term.
                        pred.Terms.Add(new Term(Regex.Replace(words[i++], @"\t|\n|\r|[()]", "")));

                        // Add the predicate to the axiom's effects.
                        axiom.Effects.Add(pred);
                    }
                }

                // Add the axiom to the set of conditional effects.
                domain.Operators.Last().Conditionals.Add(axiom);
            }
            else if (!words[i].Equals("(and"))
            {
                // Create a new effect object.
                Predicate pred = new Predicate();

                // Check for a negative effect.
                if (words[i].Equals("(not"))
                {
                    // Iterate the counter.
                    i++;

                    // Set the effect's sign to false.
                    pred.Sign = false;
                }

                // Set the effect's name.
                pred.Name = Regex.Replace(words[i], @"\t|\n|\r|[()]", "");

                // Add the effect to the operator.
                effects.Add(pred);
            }

            return i;
        }

        public static int CheckAndParseOperator(Domain domain, string [] words, int i) {
            // Fill in an operator's internal information.
            if (words[i].Equals(":parameters"))
            {
                // Add the operator's parameters.
                while (!Regex.Replace(words[i++], @"\t|\n|\r", "").Equals(":precondition"))
                    if (words[i][0] == '(' || words[i][0] == '?')
                    {
                        // Create a new term using the variable name.
                        Term term = new Term(Regex.Replace(words[i], @"\t|\n|\r|[()]", ""));

                        // Check if the term has a specified type.
                        if (Regex.Replace(words[i + 1], @"\t|\n|\r", "").Equals("-"))
                        {
                            // Iterate the counter past the dash.
                            i++;

                            // Add the type to the term object.
                            term.Type = Regex.Replace(words[++i], @"\t|\n|\r|[()]", "");
                        }

                        // Add the term to the operator's predicate.
                        domain.Operators.Last().Predicate.Terms.Add(term);
                    }

                // Create a list to hold the preconditions.
                List<IPredicate> preconditions = new List<IPredicate>();

                // Add the operator's preconditions.
                while (!Regex.Replace(words[i++], @"\t|\n|\r", "").Equals(":effect"))
                {
                    if (words[i][0] == '(')
                    {
                        if (!words[i].Equals("(and"))
                        {
                            // Create a new precondition object.
                            Predicate pred = new Predicate();

                            // Check for a negative precondition.
                            if (words[i].Equals("(not"))
                            {
                                // Iterate the counter.
                                i++;

                                // Set the effect's sign to false.
                                pred.Sign = false;
                            }

                            // Set the precondition's name.
                            pred.Name = Regex.Replace(words[i], @"\t|\n|\r|[()]", "");

                            // Add the precondition to the operator.
                            preconditions.Add(pred);
                        }
                    }
                    else
                    {
                        // Add the precondition's terms.
                        if (!Regex.Replace(words[i], @"\t|\n|\r", "").Equals(":effect") && !words[i].Equals(")"))
                            if (Regex.Replace(words[i], @"\t|\n|\r|[()]", "")[0] == '?')
                                preconditions.Last().Terms.Add(new Term(Regex.Replace(words[i], @"\t|\n|\r|[()]", "")));
                            else
                                preconditions.Last().Terms.Add(new Term(Regex.Replace(words[i], @"\t|\n|\r|[()]", ""), true));
                    }
                }

                // Add the operator's decomposition
                while (!Regex.Replace(words[i++], @"\t|\n|\r", "").Equals(":effect"))
                {
                    if (words[i][0] == '(')
                    {
                        if (!words[i].Equals("(and"))
                        {
                            // Create a new precondition object.
                            Predicate pred = new Predicate();

                            // Check for a negative precondition.
                            if (words[i].Equals("(not"))
                            {
                                // Iterate the counter.
                                i++;

                                // Set the effect's sign to false.
                                pred.Sign = false;
                            }

                            // Set the precondition's name.
                            pred.Name = Regex.Replace(words[i], @"\t|\n|\r|[()]", "");

                            // Add the precondition to the operator.
                            preconditions.Add(pred);
                        }
                    }
                    else
                    {
                        // Add the precondition's terms.
                        if (!Regex.Replace(words[i], @"\t|\n|\r", "").Equals(":effect") && !words[i].Equals(")"))
                            if (Regex.Replace(words[i], @"\t|\n|\r|[()]", "")[0] == '?')
                                preconditions.Last().Terms.Add(new Term(Regex.Replace(words[i], @"\t|\n|\r|[()]", "")));
                            else
                                preconditions.Last().Terms.Add(new Term(Regex.Replace(words[i], @"\t|\n|\r|[()]", ""), true));
                    }
                }

                // Add the preconditions to the last created operator.
                domain.Operators.Last().Preconditions = preconditions;

                // Create a list to hold the effects.
                List<IPredicate> effects = new List<IPredicate>();

                // Add the operator's effects.
                while (!Regex.Replace(words[i + 1], @"\t|\n|\r", "").Equals("(:action") && !Regex.Replace(words[i], @"\t|\n|\r", "").Equals(":agents") && i < words.Length - 2)
                {
                    if (words[i][0] == '(')
                    {
                        i = ParserHelper.CheckForConditionalEffect(domain, effects, words, i);
                    }
                    else
                    {
                        // Add the effect's terms.
                        if (!Regex.Replace(words[i], @"\t|\n|\r", "").Equals("(:action") && !words[i].Equals(")"))
                            if (Regex.Replace(words[i], @"\t|\n|\r|[()]", "")[0] == '?')
                                effects.Last().Terms.Add(new Term(Regex.Replace(words[i], @"\t|\n|\r|[()]", "")));
                            else
                                effects.Last().Terms.Add(new Term(Regex.Replace(words[i], @"\t|\n|\r|[()]", ""), true));
                    }

                    // Iterate the counter.
                    i++;
                }

                // Add the effects to the last created operator.
                domain.Operators.Last().Effects = effects;

                // Create a list for storing consenting agents.
                List<ITerm> consenting = new List<ITerm>();

                // Check if the action has any consenting agents.
                if (Regex.Replace(words[i], @"\t|\n|\r", "").Equals(":agents"))
                {
                    // If so, iterate through them.
                    while (Regex.Replace(words[++i], @"\t|\n|\r", "")[Regex.Replace(words[i], @"\t|\n|\r", "").Length - 1] != ')')
                        // And add them to the list.
                        consenting.Add(new Term(Regex.Replace(words[i], @"\t|\n|\r|[()]", "")));

                    // Add the final item to the list.
                    consenting.Add(new Term(Regex.Replace(words[i], @"\t|\n|\r|[()]", "")));
                }

                // Add the consenting agents to the action.
                domain.Operators.Last().ConsentingAgents = consenting;
            }

            return i;
        }
        
    }
}
