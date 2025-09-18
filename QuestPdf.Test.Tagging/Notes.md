# Notes – QuestPDF 2025.12.0-alpha0 (PDF Tags)

I tested the new alpha version of QuestPDF (2025.12.0-alpha0), which introduces **PDF Tags** for the first time. Here are my observations:

---

## 1. Semantic Tags for Text Elements

* Currently, **every text element** requires an explicit semantic tag.
  → This makes the code quite cluttered.
* Suggestions for improvement:

  * Add an option to **automatically generate default tags** (similar to how PDF/A mode works). Allow setting a flag in **`DocumentSettings`** that enables default semantic tags:

    * For example, calling `.Text()` would automatically use `.SemanticParagraph()` under the hood.
    * This ensures that the generated PDF always has **basic tagging** without requiring manual tagging everywhere.
  * Introduce new API elements that inherently include semantic tags:

    * instead of `.SemanticHeader1().Text()` simply `.Header1()`
    * instead of `.SemanticParagraph().Text()` simply `.Paragraph()`

---

## 2. Tables (PAC Test)

* I tested with the **PDF Accessibility Checker (PAC)**:
  👉 [PAC Website](https://pac.pdf-accessibility.org/de)
* Findings:

  * Table tags are **not yet fully correct**.
  * Data cells (`<td>`) are missing references to their associated header cells (`<th>`).
  * See more details here: [Table header cell has no associated subcells](https://support.axes4.com/hc/en-us/articles/7371462228370-Table-header-cell-has-no-associated-subcells).
* Positive note:

  * The **automatic tagging for tables** works nicely and keeps the code clean.

---

## Conclusion

* A good first implementation of PDF Tags.
* Text elements should be easier to map to semantic tags (ideally through default behavior or simplified API elements).
* Table tagging is a promising approach but still needs fixes regarding header references.
* Providing **automatic tagging through `DocumentSettings`** would help ensure accessibility without sacrificing readability in the codebase.
