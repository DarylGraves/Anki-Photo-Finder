# Anki-Photo-Finder
A basic Windows application which queries the words in a CSV against the Pexels API so you can quickly find pictures to associate with words. These pictures are downloaded and added to a new version of the Csv which can then be imported into Anki for faster card creation.

You need a (free) Pexels account for this to work.

# Notes / To Do:
- Allow CSVs with different delimiters (currently it only accepts CSVs which seperates by a comma)
- Provide better error checking and error messages (as an example a CSV with varying columns per row will just crash the application)
- Dynamic save location for the output
- Clear Cache Option needed or downloaded pictures persist indefinitely.
- Clean up - I went from trying to everything to a very high standard to rushing to get this working so I can move onto other projects. I'm proud it works, but can't say the same about the underlying code structure.
