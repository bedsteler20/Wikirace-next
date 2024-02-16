import re
import subprocess

# The path to your Markdown file
markdown_file_path = "TODO.md"

# The regex pattern to match TODOs
pattern = r"- \[ \] (.*)"

# Open the Markdown file
with open(markdown_file_path, "r") as file:
    # Read the file
    content = file.read()

    # Find all TODOs
    todos = re.findall(pattern, content)

    # For each TODO
    for todo in todos:
        # The command to create an issue
        command = ["gh", "issue", "create", "--title", todo, "--body", ""]

        # Run the command
        result = subprocess.run(command, capture_output=True, text=True)

        # If the command was successful
        if result.returncode == 0:
            print(f'Successfully created issue "{todo}"')
        else:
            print(f'{result.stderr}')
            print(f'Failed to create issue "{todo}"')
